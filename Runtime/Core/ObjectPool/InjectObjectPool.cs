using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    /// <summary>
    /// game object pool that injects on instantiation
    /// </summary>
    public static class InjectObjectPool
    {
        private static Transform CreateRoot(GameObject prefab, bool isPersistent)
        {
            var root = new GameObject($"InjectObjectPool {prefab.name}");

            if (isPersistent)
                UnityEngine.Object.DontDestroyOnLoad(root);

            root.SetActive(false);
            return root.transform;
        }

        public static IInjectObjectPool Create(GameObject prefab, bool isPersistent = true)
        {
            return Create(prefab, Pin.DefaultInstantiator, isPersistent : isPersistent);
        }

        public static IInjectObjectPool Create(
            GameObject prefab, int minInstance, int maxInstance, bool isPersistent = true)
        {
            return Create(prefab, Pin.DefaultInstantiator, minInstance, maxInstance, isPersistent);
        }

        public static IInjectObjectPool Create(
            GameObject prefab, Pin.InstantiatorDelegate instantiator,
            int minInstance = 0, int maxInstance = 100, bool isPersistent = true)
        {
            var root = CreateRoot(prefab, isPersistent);
            return new InjectObjectPoolImpl(root, prefab, minInstance, maxInstance, instantiator);
        }

        public static IInjectObjectPool<T> Create<T>(T prefab, bool isPersistent = true)
            where T : Component
        {
            return Create(prefab, Pin.DefaultInstantiator, isPersistent : isPersistent);
        }

        public static IInjectObjectPool<T> Create<T>(
                T prefab, int minInstance, int maxInstance, bool isPersistent = true)
            where T : Component
        {
            return Create(prefab, Pin.DefaultInstantiator, minInstance, maxInstance, isPersistent);
        }

        public static IInjectObjectPool<T> Create<T>(
                T prefab, Pin.InstantiatorDelegate instantiator,
                int minInstance = 0, int maxInstance = 100, bool isPersistent = true)
            where T : Component
        {
            var root = CreateRoot(prefab.gameObject, isPersistent);
            return new InjectObjectPoolImpl<T>(root, prefab, minInstance, maxInstance, instantiator);
        }
    }
}
