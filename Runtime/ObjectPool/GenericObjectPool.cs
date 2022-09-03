using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject
{
    /// <summary>
    /// A generic object pool for C# object.
    /// </summary>
    public class GenericObjectPool<T> where T : class, new()
    {
        private Stack<T> _pool = new Stack<T>();

        private readonly int _maxInstance;

        public GenericObjectPool(int minInstance = 0, int maxInstance = 100)
        {
            _maxInstance = maxInstance;

            for (int i = 0; i < minInstance; i++)
                _pool.Push(CreateInstance());
        }

        public T Get()
        {
            T instance;

            if (_pool.Count > 0)
                instance = _pool.Pop();
            else
                instance = CreateInstance();
            return instance;
        }

        public void Release(T instance)
        {
            if (_pool.Count < _maxInstance)
            {
                ResetInstance(instance);
                _pool.Push(instance);
            }
            else
            {
                DisposeInstance(instance);
            }
        }

        protected virtual T CreateInstance() { return new T(); }

        protected virtual void ResetInstance(T instance) { }

        protected virtual void DisposeInstance(T instance) { }
    }
}
