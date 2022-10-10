// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using UnityEngine;

namespace Cathei.PinInject
{
    /// <summary>
    /// Game object pool that injects on Spawn.
    /// </summary>
    public interface IAutoInjectObjectPool<T> : IDisposable
    {
        int CountInactive { get; }

        T Spawn(Transform parent, Pin.ContextConfiguration config = null);
        T Spawn(Vector3 position, Quaternion rotation,
            Transform parent = null, bool worldSpace = true, Pin.ContextConfiguration config = null);

        void Despawn(T instance);
        void Clear();
    }

    public interface IAutoInjectObjectPool : IAutoInjectObjectPool<GameObject> { }
}