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
            return Create(() => new T(), Ignore, Ignore, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Func<T> createInstance, int minInstance = 0, int maxInstance = 100)
            where T : class
        {
            return Create(createInstance, Ignore, Ignore, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Action<T> resetInstance, int minInstance = 0, int maxInstance = 100)
            where T : class, new()
        {
            return Create(() => new T(), resetInstance, Ignore, minInstance, maxInstance);
        }

        public static IObjectPool<T> Create<T>(
                Func<T> createInstance, Action<T> resetInstance, Action<T> disposeInstance,
                int minInstance = 0, int maxInstance = 100)
            where T : class
        {
            return new GenericObjectPoolImpl<T>(
                minInstance, maxInstance, createInstance, resetInstance ?? Ignore, disposeInstance ?? Ignore);
        }
    }
}
