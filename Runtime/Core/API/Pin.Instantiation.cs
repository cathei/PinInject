// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject.Internal;
using UnityEngine;
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

        /// <summary>
        /// Instantiate and inject a GameObject. Context of parent transform will be used.
        /// </summary>
        public static GameObject Instantiate(
            GameObject prefab, Transform parent = null, ContextConfiguration config = null, Instantiator instantiator = null)
        {
            return InstantiateInternal(prefab, parent, new TransformArgs
            {
                position = prefab.transform.position,
                rotation = prefab.transform.rotation,
                worldSpace = false
            }, config, instantiator);
        }

        /// <summary>
        /// Instantiate and inject a GameObject. Context of parent transform will be used.
        /// </summary>
        public static GameObject Instantiate(
            GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true,
            ContextConfiguration config = null, Instantiator instantiator = null)
        {
            return InstantiateInternal(prefab, parent, new TransformArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            }, config, instantiator);
        }

        /// <summary>
        /// Instantiate and inject a GameObject. Context of parent transform will be used.
        /// </summary>
        public static T Instantiate<T>(
                T prefab, Transform parent = null, ContextConfiguration config = null, Instantiator instantiator = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, parent, config, instantiator).GetComponent<T>();
        }

        /// <summary>
        /// Instantiate and inject a GameObject. Context of parent transform will be used.
        /// </summary>
        public static T Instantiate<T>(
                T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true,
                ContextConfiguration config = null, Instantiator instantiator = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, position, rotation, parent, worldSpace, config, instantiator).GetComponent<T>();
        }

        internal static GameObject DefaultInstantiator(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }

        private static GameObject InstantiateInternal(
            GameObject prefab, Transform parent, TransformArgs args, ContextConfiguration config, Instantiator instantiator)
        {
            instantiator ??= DefaultInstantiator;

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
