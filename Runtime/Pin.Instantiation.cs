using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        internal struct PositionArgs
        {
            public Vector3 position;
            public Quaternion rotation;
            public bool worldSpace;

            public void Apply(Transform transform)
            {
                if (worldSpace)
                {
                    transform.position = position;
                    transform.rotation = rotation;
                }
                else
                {
                    transform.localPosition = position;
                    transform.localRotation = rotation;
                }
            }
        }

        public delegate GameObject InstantiatorDelegate(GameObject prefab, Transform parent);

        public static GameObject Instantiate(
            GameObject prefab, Transform parent = null, InstantiatorDelegate instantiator = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new PositionArgs
            {
                position = prefab.transform.position,
                rotation = prefab.transform.rotation,
                worldSpace = false
            }, instantiator);
        }

        public static GameObject Instantiate(
            GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true, InstantiatorDelegate instantiator = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new PositionArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            }, instantiator);
        }

        public static T Instantiate<T>(
            T prefab, Transform parent = null, InstantiatorDelegate instantiator = null) where T : Component
        {
            return Instantiate(prefab.gameObject, parent, instantiator).GetComponent<T>();
        }

        public static T Instantiate<T>(
            T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true, InstantiatorDelegate instantiator = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, position, rotation, parent, worldSpace, instantiator).GetComponent<T>();
        }

        public static void Inject(GameObject obj)
        {
            _injectStrategy.Inject(obj, GetSceneContainer(obj.scene));
        }

        public static void Inject<TComponent>(TComponent obj)
            where TComponent : Component
        {
            Inject(obj.gameObject);
        }

        internal static GameObject DefaultInstantiator(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        private static GameObject InstantiateInternal(
            GameObject prefab, Transform parent, PositionArgs args, InstantiatorDelegate instantiator)
        {
            bool savedActiveSelf = prefab.activeSelf;

            try
            {
                // turn off prefab to make sure Awake() is not called before injection
                prefab.SetActive(false);

                // prefab cached components
                _injectStrategy.CacheInnerReferences(prefab);

                var instance = instantiator(prefab, parent);

                args.Apply(instance.transform);

                Inject(instance);

                instance.SetActive(savedActiveSelf);

                return instance;
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }
        }
    }
}
