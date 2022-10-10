// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    public sealed class UnityInjectionStrategy : IInjectionStrategy<GameObject>
    {
        internal const HideFlags HiddenComponentFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;

        public void Inject(GameObject gameObject, IDependencyContainer baseContainer, Pin.ContextConfiguration config)
        {
            var cacheComponent = gameObject.CacheInnerReferences();
            baseContainer = gameObject.FindParentContainer() ?? baseContainer;

            foreach (var node in cacheComponent.InnerReferences)
            {
                if (node.container == null)
                    continue;

                node.container.Container.Reset();
            }

            if (config != null)
            {
                // local context should be created
                var localContainer = gameObject.GetOrAddContainerComponent().Container;

                localContainer.Reset();
                localContainer.SetParent(baseContainer);

                var binder = new DependencyBinder(localContainer);
                config(binder);

                baseContainer = localContainer;
            }

            foreach (var node in cacheComponent.InnerReferences)
            {
                IDependencyContainer container;
                DependencyBinder binder;

                if (node.container != null)
                {
                    var localContainer = node.container.Container;

                    localContainer.SetParent(
                        node.parent != null ? node.parent.Container : baseContainer);

                    container = node.container.Container;
                    binder = new DependencyBinder(node.container.Container);
                }
                else
                {
                    container = baseContainer;
                    binder = default;
                }

                foreach (var component in node.components)
                    Pin.DefaultStrategy.InjectBindResolve(component, container, binder);
            }

            // when it's injected, references should be invalidated
            cacheComponent.IsValid = false;
        }
    }

    internal static class UnityStrategyExtensions
    {
        // unity itself is single-threaded so we can just use single temp variable
        private static readonly List<MonoBehaviour> ComponentBuffer = new List<MonoBehaviour>();

        internal static DependencyContainerComponent GetOrAddContainerComponent(this GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out DependencyContainerComponent component))
            {
                component = gameObject.AddComponent<DependencyContainerComponent>();
                component.hideFlags = UnityInjectionStrategy.HiddenComponentFlags;
            }

            return component;
        }

        internal static DependencyContainer FindParentContainer(this GameObject gameObject)
        {
            DependencyContainer parentContainer = null;

            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                if (parent.TryGetComponent(out DependencyContainerComponent component))
                {
                    parentContainer = component.Container;
                    break;
                }

                parent = parent.parent;
            }

            return parentContainer;
        }

        public static HierarchyCacheComponent CacheInnerReferences(this GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out HierarchyCacheComponent component))
            {
                component = gameObject.AddComponent<HierarchyCacheComponent>();
                component.hideFlags = UnityInjectionStrategy.HiddenComponentFlags;
            }

            if (!component.IsValid)
            {
                component.InnerReferences.Clear();

                // create root node
                var node = new HierarchyCacheComponent.Node
                {
                    container = null,
                    parent = null,
                    components = new List<MonoBehaviour>()
                };

                component.InnerReferences.Add(node);

                CacheInnerReferencesInternal(component, component.transform, node);

                component.IsValid = true;
            }

            return component;
        }

        // can be called for prefab (Instantiate) or instance (Inject)
        // prefab version is recommended for performance
        private static void CacheInnerReferencesInternal(
            HierarchyCacheComponent cache, Transform target, HierarchyCacheComponent.Node parentNode)
        {
            HierarchyCacheComponent.Node node = parentNode;

            if (target.TryGetComponent(out IInjectionContext _) || target.TryGetComponent(out ICompositionRoot _))
            {
                // local context exists for this game object
                var localContainer = target.gameObject.GetOrAddContainerComponent();

                node = new HierarchyCacheComponent.Node
                {
                    container = localContainer,
                    parent = parentNode.container,
                    components = new List<MonoBehaviour>()
                };

                cache.InnerReferences.Add(node);
            }

            target.GetComponents(ComponentBuffer);

            // contexts should be injected first
            foreach (var component in ComponentBuffer)
            {
                if (component is IInjectionContext)
                    node.components.Add(component);
            }

            foreach (var component in ComponentBuffer)
            {
                // it is already included
                if (component is IInjectionContext)
                    continue;

                var reflection = ReflectionCache.Get(component.GetType());

                if (reflection.HasAnyAttribute)
                    node.components.Add(component);
            }

            for (int i = 0; i < target.childCount; ++i)
            {
                Transform child = target.GetChild(i);
                CacheInnerReferencesInternal(cache, child, node);
            }
        }

    }
}
