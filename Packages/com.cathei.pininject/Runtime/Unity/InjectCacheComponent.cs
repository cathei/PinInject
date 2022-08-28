using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to Prefab and cache innner reference to injectable Unity Object.
    /// </summary>
    public class InjectCacheComponent : MonoBehaviour
    {
        // [Serializable]
        // public struct 

        [Serializable]
        public struct InnerPrefabReferences
        {
            public InjectContainerComponent container;
            public MonoBehaviour component;
        }

        [SerializeField]
        private List<InnerPrefabReferences> _innerReferences = new List<InnerPrefabReferences>();

        [SerializeField]
        private bool _isValid = false;

        // unity itself is single-threaded so just have temp variable as static
        private static readonly List<MonoBehaviour> _tempComponents = new List<MonoBehaviour>();

        // can be only called on prefab (Instantiate) or instance (Inject)
        // prefab version is recommended for performance
        internal void CacheComponents()
        {
            if (_isValid)
                return;

            _innerReferences.Clear();
            CacheComponentInternal(transform, null);

            _isValid = true;
        }

        private void CacheComponentInternal(Transform target, InjectContainerComponent parentContainer)
        {
            InjectContainerComponent container = parentContainer;

            if (target.GetComponent<IInjectContext>() != null)
            {
                // child container will be used for this game object
                var childContainer = InjectContainerComponent.GetOrCreate(target.gameObject);
                childContainer.parent = parentContainer;
                container = childContainer;

                // container referencing itself
                _innerReferences.Add(new InnerPrefabReferences
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

                var cache = ReflectionCache.Get(component.GetType());

                if (cache.HasAnyAttribute || component is IInjectContext)
                {
                    _innerReferences.Add(new InnerPrefabReferences
                    {
                        container = container,
                        component = component
                    });
                }
            }

            for (int i = 0; i < target.childCount; ++i)
            {
                Transform child = target.GetChild(i);
                CacheComponentInternal(child, container);
            }
        }

        // should be only called on instance
        internal void InjectComponents(InjectContainer rootContainer)
        {
            if (!_isValid)
                CacheComponents();

            foreach (var reference in _innerReferences)
            {
                InjectContainer container = reference.container?._container;

                if (container != null)
                {
                    if (reference.container == reference.component)
                    {
                        var parent = reference.container.parent?._container ?? rootContainer;

                        container.Reset();
                        container.SetParent(parent);
                    }

                    if (reference.component is IInjectContext context)
                    {
                        context.Configure(container);
                    }
                }
                else
                {
                    container = rootContainer;
                }

                var cache = ReflectionCache.Get(reference.component.GetType());
                cache.Inject(reference.component, container);
            }

            // when it's injected, references should be invalidated
            _isValid = false;
        }

        public static InjectCacheComponent CacheReferences(GameObject gameObject)
        {
            var component = gameObject.GetComponent<InjectCacheComponent>();
            if (component != null)
                return component;

            component = gameObject.AddComponent<InjectCacheComponent>();
            component.hideFlags = HideFlags.HideAndDontSave;
            component.CacheComponents();

            return component;
        }
    }
}
