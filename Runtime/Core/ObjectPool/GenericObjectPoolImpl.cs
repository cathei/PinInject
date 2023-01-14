// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// A generic object pool for C# object.
    /// </summary>
    internal class GenericObjectPoolImpl<T> : IObjectPool<T>, IDisposable
        where T : class
    {
        private Stack<T> _pool = new Stack<T>();

        private readonly int _maxInstance;

        public int CountInactive => _pool.Count;

        private readonly Func<T> _createInstance;
        private readonly Action<T> _resetInstance;
        private readonly Action<T> _disposeInstance;

        internal GenericObjectPoolImpl(int minInstance, int maxInstance,
            Func<T> createInstance, Action<T> resetInstance, Action<T> disposeInstance)
        {
            _maxInstance = maxInstance;

            _createInstance = createInstance;
            _resetInstance = resetInstance;
            _disposeInstance = disposeInstance;

            for (int i = 0; i < minInstance; i++)
            {
                _pool.Push(_createInstance());
            }
        }

        public T Get()
        {
            T instance;

            if (_pool.Count > 0)
                instance = _pool.Pop();
            else
                instance = _createInstance();
            return instance;
        }

        public PooledObject<T> Get(out T value)
        {
            value = Get();
            return new PooledObject<T>(value, this);
        }

        public void Release(T instance)
        {
            if (_pool.Count < _maxInstance)
            {
                _resetInstance(instance);
                _pool.Push(instance);
            }
            else
            {
                _disposeInstance(instance);
            }
        }

        public void Clear()
        {
            foreach (T item in _pool)
                _disposeInstance(item);

            _pool.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
