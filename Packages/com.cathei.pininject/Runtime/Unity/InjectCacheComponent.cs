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
        [Serializable]
        public struct InnerPrefabReferences
        {
            public InjectContainerComponent container;
            public MonoBehaviour unityObject;
        }

        [SerializeField]
        private List<InnerPrefabReferences> _innerReferences = null;

        // unity itself is single-threaded so just have temp variable as static
        private static readonly List<MonoBehaviour> _tempComponents = new List<MonoBehaviour>();

        // can be only called on prefab (Instantiate) or instance (Inject)
        // prefab version is recommended for performance
        internal void CacheComponents()
        {
            if (_innerReferences != null)
                return;

            _innerReferences = new List<InnerPrefabReferences>();

            GetComponentsInChildren(true, _tempComponents);

            foreach (var component in _tempComponents)
            {
                var cache = ReflectionCache.Get(component.GetType());

                if (cache.HasAnyAttribute)
                    _innerReferences.Add(new InnerPrefabReferences { unityObject = component });
            }
        }

        // should be only called on instance
        internal void InjectComponents(InjectContainer container)
        {
            if (_innerReferences == null)
                CacheComponents();

            foreach (var reference in _innerReferences)
            {
                var cache = ReflectionCache.Get(reference.unityObject.GetType());
                cache.Inject(reference.unityObject, container);
            }

            // when it's injected, references should be invalidated
            _innerReferences.Clear();
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
