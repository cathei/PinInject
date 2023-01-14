using System;

#if !UNITY_2021_1_OR_NEWER

namespace UnityEngine.Pool
{
    public struct PooledObject<T> : IDisposable
        where T : class
    {
        private readonly T value;
        private readonly IObjectPool<T> pool;

        internal PooledObject(T value, IObjectPool<T> pool)
        {
            this.value = value;
            this.pool = pool;
        }

        void IDisposable.Dispose() => pool.Release(this.value);
    }

    public interface IObjectPool<T> where T : class
    {
        int CountInactive { get; }

        T Get();
        PooledObject<T> Get(out T value);

        void Release(T instance);
        void Clear();
    }
}

#endif


