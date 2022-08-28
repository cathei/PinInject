using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        public static GameObject Instantiate(
            GameObject prefab, Transform parent = null,
            Func<GameObject, Transform, GameObject> instantiator = null)
        {
            instantiator ??= DefaultInstantiator;
            return InstantiateInternal(prefab, parent, instantiator);
        }

        public static T Instantiate<T>(T prefab, Transform parent = null)
            where T : Component
        {
            return InstantiateInternal(prefab.gameObject, parent, DefaultInstantiator).GetComponent<T>();
        }

        public static void Inject(GameObject obj)
        {
            InjectInternal(obj);
        }

        private static GameObject DefaultInstantiator(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        private static GameObject InstantiateInternal(
            GameObject prefab, Transform parent, Func<GameObject, Transform, GameObject> instantiator)
        {
            bool savedActiveSelf = prefab.activeSelf;

            try
            {
                // turn off prefab to make sure Awake() is not called before injection
                prefab.SetActive(false);

                // prefab cached components
                InjectCacheComponent.CacheReferences(prefab);

                var instance = instantiator(prefab, parent);

                InjectInternal(instance);

                instance.SetActive(savedActiveSelf);

                return instance;
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }
        }

        private static void InjectInternal(GameObject instance)
        {
            // instance cached components
            var cache = InjectCacheComponent.CacheReferences(instance);

            // find parent context
            var container = InjectContainerComponent.FindParentContainer(instance.transform);

            cache.InjectComponents(container);
        }
    }
    
}