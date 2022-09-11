#if !UNITY_2021_1_OR_NEWER

using System;
using UnityEngine;

namespace UnityEngine
{
    public interface IObjectPool<T>
    {
        int CountInactive { get; }

        T Get();
        void Release(T instance);
        void Clear();
    }
}

#endif

namespace Cathei.PinInject
{
    public interface IInjectObjectPool<T> : IDisposable
    {
        int CountInactive { get; }

        T Spawn(Transform parent);
        T Spawn(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true);

        void Despawn(T instance);
        void Clear();
    }

    public interface IInjectObjectPool : IInjectObjectPool<GameObject> { }
}


