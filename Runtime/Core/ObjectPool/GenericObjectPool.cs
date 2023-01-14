// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.Pool;

namespace Cathei.PinInject
{
    public static class GenericObjectPool
    {
        private static void Ignore<T>(T instance) { }

        public static IObjectPool<T> Create<T>(int minInstance = 0, int maxInstance = 100)
            where T : class, new()
        {
            return Create(() => new T(), null, null, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Func<T> createInstance, int minInstance = 0, int maxInstance = 100)
            where T : class
        {
            return Create(createInstance, null, null, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Action<T> resetInstance, int minInstance = 0, int maxInstance = 100)
            where T : class, new()
        {
            return Create(() => new T(), resetInstance, null, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Func<T> createInstance, Action<T> resetInstance, Action<T> disposeInstance,
                int minInstance = 0, int maxInstance = 100)
            where T : class
        {
            var objectPool = new ObjectPool<T>(createInstance, null, resetInstance, disposeInstance,
                defaultCapacity: minInstance, maxSize: maxInstance);

            if (minInstance > 0)
            {
                // fill object pool, Unity only allocate for list capacity
                for (int i = 0; i < minInstance; ++i)
                    objectPool.Release(createInstance());
            }

            return objectPool;
        }
    }
}
