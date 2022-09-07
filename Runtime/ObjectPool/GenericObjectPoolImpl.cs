using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
