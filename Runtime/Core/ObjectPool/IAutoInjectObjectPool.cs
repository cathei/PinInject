// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using UnityEngine;

namespace Cathei.PinInject
{
    public interface IAutoInjectObjectPool<T> : IDisposable
    {
        int CountInactive { get; }

        T Spawn(Transform parent);
        T Spawn(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true);

        void Despawn(T instance);
        void Clear();
    }

    public interface IAutoInjectObjectPool : IAutoInjectObjectPool<GameObject> { }
}