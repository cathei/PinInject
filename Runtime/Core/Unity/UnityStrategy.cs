// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    public class UnityStrategy : DefaultStrategy
    {
        internal const HideFlags HiddenComponentFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;

        public override void Inject(object obj, IDependencyContainer container)
        {
            if (obj is GameObject go)
            {
                InjectGameObject(go, container);
                return;
            }

            base.Inject(obj, container);
        }

        private void InjectGameObject(GameObject gameObject, IDependencyContainer baseContainer)
        {
            var cacheComponent = gameObject.CacheInnerReferences();
            baseContainer = gameObject.FindParentContainer() ?? baseContainer;

            foreach (var reference in cacheComponent.InnerReferences)
            {
                IDependencyContainer container;
                DependencyRegistry registry;

                if (reference.container != null)
                {
                    if (reference.container == reference.component)
                    {
                        var parent = baseContainer;

                        if (reference.container.parent != null)
                            parent = reference.container.parent.Container;

                        reference.container.Container.Reset();
                        reference.container.Container.SetParent(parent);
                    }

                    container = reference.container.Container;
                    registry = new DependencyRegistry(reference.container.Container);
                }
                else
                {
                    container = baseContainer;
                    registry = default;
                }

                InjectBindResolve(reference.component, container, registry);
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
                component.hideFlags = UnityStrategy.HiddenComponentFlags;
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
                component.hideFlags = UnityStrategy.HiddenComponentFlags;
            }

            if (!component.IsValid)
            {
                component.InnerReferences.Clear();
                CacheInnerReferencesInternal(component, component.transform, null);
                component.IsValid = true;
            }

            return component;
        }

        // can be called for prefab (Instantiate) or instance (Inject)
        // prefab version is recommended for performance
        private static void CacheInnerReferencesInternal(HierarchyCacheComponent cache, Transform target, DependencyContainerComponent parentContainer)
        {
            DependencyContainerComponent container = parentContainer;

            if (target.TryGetComponent(out IContext _) || target.TryGetComponent(out ICompositionRoot _))
            {
                // child container will be used for this game object
                container = target.gameObject.GetOrAddContainerComponent();
                container.parent = parentContainer;

                // container referencing itself
                cache.InnerReferences.Add(new HierarchyCacheComponent.InnerPrefabReferences
                {
                    container = container,
                    component = container
                });
            }

            target.GetComponents(ComponentBuffer);

            // contexts should be injected first
            foreach (var component in ComponentBuffer)
            {
                if (component is IContext)
                {
                    cache.InnerReferences.Add(new HierarchyCacheComponent.InnerPrefabReferences
                    {
                        container = container,
                        component = component
                    });
                }
            }

            foreach (var component in ComponentBuffer)
            {
                // it is already included
                if (component is IContext)
                    continue;

                var reflection = ReflectionCache.Get(component.GetType());

                if (reflection.HasAnyAttribute)
                {
                    cache.InnerReferences.Add(new HierarchyCacheComponent.InnerPrefabReferences
                    {
                        container = container,
                        component = component
                    });
                }
            }

            for (int i = 0; i < target.childCount; ++i)
            {
                Transform child = target.GetChild(i);
                CacheInnerReferencesInternal(cache, child, container);
            }
        }

    }
}
