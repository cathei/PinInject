// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    /// <summary>
    /// Game object pool that injects on Spawn.
    /// </summary>
    public static class AutoInjectObjectPool
    {
        private static Transform CreateRoot(GameObject prefab, bool isPersistent)
        {
            var root = new GameObject($"InjectObjectPool {prefab.name}");

            if (isPersistent)
                Object.DontDestroyOnLoad(root);

            root.SetActive(false);
            return root.transform;
        }

        public static IAutoInjectObjectPool Create(GameObject prefab, bool isPersistent = true)
        {
            return Create(prefab, Pin.DefaultInstantiator, isPersistent : isPersistent);
        }

        public static IAutoInjectObjectPool Create(
            GameObject prefab, int minInstance, int maxInstance, bool isPersistent = true)
        {
            return Create(prefab, Pin.DefaultInstantiator, minInstance, maxInstance, isPersistent);
        }

        public static IAutoInjectObjectPool Create(
            GameObject prefab, Pin.Instantiator instantiator,
            int minInstance = 0, int maxInstance = 100, bool isPersistent = true)
        {
            var root = CreateRoot(prefab, isPersistent);
            return new AutoInjectObjectPoolImpl(root, prefab, minInstance, maxInstance, instantiator);
        }

        public static IAutoInjectObjectPool<T> Create<T>(T prefab, bool isPersistent = true)
            where T : Component
        {
            return Create(prefab, Pin.DefaultInstantiator, isPersistent : isPersistent);
        }

        public static IAutoInjectObjectPool<T> Create<T>(
                T prefab, int minInstance, int maxInstance, bool isPersistent = true)
            where T : Component
        {
            return Create(prefab, Pin.DefaultInstantiator, minInstance, maxInstance, isPersistent);
        }

        public static IAutoInjectObjectPool<T> Create<T>(
                T prefab, Pin.Instantiator instantiator,
                int minInstance = 0, int maxInstance = 100, bool isPersistent = true)
            where T : Component
        {
            var root = CreateRoot(prefab.gameObject, isPersistent);
            return new AutoInjectObjectPoolImpl<T>(root, prefab, minInstance, maxInstance, instantiator);
        }
    }
}
