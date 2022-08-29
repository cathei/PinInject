using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    public class UnityInjectStrategy : IInjectStrategy
    {
        private readonly DefaultInjectStrategy _defaultStrategy = new DefaultInjectStrategy();

        // unity itself is single-threaded so we can just use single temp variable
        private readonly List<MonoBehaviour> _tempComponents = new List<MonoBehaviour>();

        public void Inject(object obj, InjectContainer container)
        {
            if (obj is GameObject go)
            {
                InjectGameObject(go, container);
                return;
            }

            _defaultStrategy.Inject(obj, container);
        }

        private void InjectGameObject(GameObject gameObject, InjectContainer baseContainer)
        {
            var cacheComponent = CacheInnerReferences(gameObject);
            baseContainer = FindParentContainer(gameObject.transform) ?? baseContainer;

            foreach (var reference in cacheComponent.InnerReferences)
            {
                InjectContainer container = reference.container?._container;

                if (container != null)
                {
                    if (reference.container == reference.component)
                    {
                        var parent = reference.container.parent?._container ?? baseContainer;

                        container.Reset();
                        container.SetParent(parent);
                    }
                }
                else
                {
                    container = baseContainer;
                }

                var cache = ReflectionCache.Get(reference.component.GetType());

                _defaultStrategy.InjectProperties(cache, reference.component, container);

                if (reference.component is IInjectContext context)
                    context.Configure(container);

                _defaultStrategy.ResolveProperties(cache, reference.component, container);

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
                component.hideFlags = HideFlags.HideAndDontSave;
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
            InjectContainerComponent container = parentContainer;

            if (target.GetComponent<IInjectContext>() != null)
            {
                // child container will be used for this game object
                var childContainer = GetContainerComponent(target.gameObject);
                childContainer.parent = parentContainer;
                container = childContainer;

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

                // ignore scene inject component
                if (component is ISceneInjectContext)
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
                component.hideFlags = HideFlags.HideAndDontSave;
            }

            return component;
        }

        private InjectContainer FindParentContainer(Transform transform)
        {
            InjectContainer parentContainer = null;

            Transform parent = transform;

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
