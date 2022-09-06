using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    public class UnityInjectStrategy : DefaultInjectStrategy
    {
        // unity itself is single-threaded so we can just use single temp variable
        private readonly List<MonoBehaviour> _tempComponents = new List<MonoBehaviour>();

        private const HideFlags hiddenComponentFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;

        public override void Inject(object obj, IInjectContainer container)
        {
            if (obj is GameObject go)
            {
                InjectGameObject(go, container);
                return;
            }

            base.Inject(obj, container);
        }

        private void InjectGameObject(GameObject gameObject, IInjectContainer baseContainer)
        {
            var cacheComponent = CacheInnerReferences(gameObject);
            baseContainer = FindParentContainer(gameObject.transform) ?? baseContainer;

            foreach (var reference in cacheComponent.InnerReferences)
            {
                InjectContainerImpl containerImpl = reference.container?._container;
                IInjectContainer container = containerImpl;

                if (container != null)
                {
                    if (reference.container == reference.component)
                    {
                        var parent = reference.container.parent?._container ?? baseContainer;

                        containerImpl.Reset();
                        containerImpl.SetParent(parent);
                    }
                }
                else
                {
                    container = baseContainer;
                }

                var cache = ReflectionCache.Get(reference.component.GetType());

                InjectProperties(cache, reference.component, container);

                if (reference.component is IInjectContext context)
                    context.Configure(containerImpl);

                ResolveProperties(cache, reference.component, container);

                if (reference.component is IPostInjectHandler postInjectHandler)
                    postInjectHandler.PostInject();
            }

            // when it's injected, references should be invalidated
            cacheComponent.IsValid = false;
        }

        public InjectCacheComponent CacheInnerReferences(GameObject gameObject)
        {
            var component = gameObject.GetComponent<InjectCacheComponent>();

            if (component == null)
            {
                component = gameObject.AddComponent<InjectCacheComponent>();
                component.hideFlags = hiddenComponentFlags;
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
        private void CacheInnerReferencesInternal(InjectCacheComponent cache, Transform target, InjectContainerComponent parentContainer)
        {
            // scene inject root should not be affected
            if (target.GetComponent<SceneInjectRoot>())
                return;

            InjectContainerComponent container = parentContainer;

            if (target.GetComponent<IInjectContext>() != null)
            {
                // child container will be used for this game object
                container = GetContainerComponent(target.gameObject);
                container.parent = parentContainer;

                // container referencing itself
                cache.InnerReferences.Add(new InjectCacheComponent.InnerPrefabReferences
                {
                    container = container,
                    component = container
                });
            }

            target.GetComponents(_tempComponents);

            // contexts should be injected first
            _tempComponents.Sort((x, y) => x is IInjectContext ? -1 : 1);

            foreach (var component in _tempComponents)
            {
                // it is already included
                if (component is InjectContainerComponent)
                    continue;

                var reflection = ReflectionCache.Get(component.GetType());

                if (reflection.HasAnyAttribute || component is IInjectContext)
                {
                    cache.InnerReferences.Add(new InjectCacheComponent.InnerPrefabReferences
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

        private InjectContainerComponent GetContainerComponent(GameObject gameObject)
        {
            var component = gameObject.GetComponent<InjectContainerComponent>();

            if (component == null)
            {
                component = gameObject.AddComponent<InjectContainerComponent>();
                component.hideFlags = hiddenComponentFlags;
            }

            return component;
        }

        private InjectContainerImpl FindParentContainer(Transform transform)
        {
            InjectContainerImpl parentContainer = null;

            Transform parent = transform.parent;

            while (parent != null)
            {
                var component = parent.GetComponent<InjectContainerComponent>();

                if (component != null)
                {
                    parentContainer = component._container;
                    break;
                }

                parent = parent.parent;
            }

            return parentContainer;
        }
    }
}
