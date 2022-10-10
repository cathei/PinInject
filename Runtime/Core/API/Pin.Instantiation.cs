// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        internal struct TransformArgs
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

        public delegate GameObject Instantiator(GameObject prefab, Transform parent);

        public static GameObject Instantiate(
            GameObject prefab, Transform parent = null, Instantiator instantiator = null, ContextConfiguration config = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new TransformArgs
            {
                position = prefab.transform.position,
                rotation = prefab.transform.rotation,
                worldSpace = false
            }, instantiator, config);
        }

        public static GameObject Instantiate(
            GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true,
            Instantiator instantiator = null, ContextConfiguration config = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new TransformArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            }, instantiator, config);
        }

        public static T Instantiate<T>(
                T prefab, Transform parent = null, Instantiator instantiator = null, ContextConfiguration config = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, parent, instantiator, config).GetComponent<T>();
        }

        public static T Instantiate<T>(
                T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true,
                Instantiator instantiator = null, ContextConfiguration config = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, position, rotation, parent, worldSpace, instantiator, config).GetComponent<T>();
        }

        internal static GameObject DefaultInstantiator(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }

        private static GameObject InstantiateInternal(
            GameObject prefab, Transform parent, TransformArgs args, Instantiator instantiator, ContextConfiguration config)
        {
            bool savedActiveSelf = prefab.activeSelf;

            try
            {
                // turn off prefab to make sure Awake() is not called before injection
                prefab.SetActive(false);

                // prefab cached components
                prefab.CacheInnerReferences();

                var instance = instantiator(prefab, parent);

                args.Apply(instance.transform);

                InjectGameObjectInternal(instance, config);

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
