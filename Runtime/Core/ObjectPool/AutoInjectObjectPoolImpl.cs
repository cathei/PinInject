// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// game object pool that injects on instantiation
    /// </summary>
    internal class AutoInjectObjectPoolImpl : IAutoInjectObjectPool
    {
        private readonly Transform _root;
        private readonly GameObject _prefab;
        private readonly IObjectPool<GameObject> _pool;
        private readonly Pin.Instantiator _instantiator;

        internal AutoInjectObjectPoolImpl(
            Transform root, GameObject prefab, int minInstance, int maxInstance,
            Pin.Instantiator instantiator)
        {
            _root = root;
            _prefab = prefab;
            _instantiator = instantiator;

            // inactivated root is required
            _root.gameObject.SetActive(false);

            _pool = GenericObjectPool.Create(
                CreateInstance, ResetInstance, DisposeInstance, minInstance, maxInstance);
        }

        public int CountInactive => _pool.CountInactive;

        public GameObject Spawn(Transform parent)
        {
            return SpawnInternal(parent, new Pin.TransformArgs
            {
                position = _prefab.transform.position,
                rotation = _prefab.transform.rotation,
                worldSpace = false
            });
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true)
        {
            return SpawnInternal(parent, new Pin.TransformArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            });
        }

        private GameObject SpawnInternal(Transform parent, Pin.TransformArgs args)
        {
            var instance = _pool.Get();

            instance.SetActive(false);

            instance.transform.SetParent(parent);

            args.Apply(instance.transform);

            Pin.Inject(instance);

            instance.SetActive(true);
            return instance;
        }

        public void Despawn(GameObject instance)
        {
            _pool.Release(instance);
        }

        public void Clear()
        {
            _pool.Clear();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_root.gameObject);
        }

        private GameObject CreateInstance()
        {
            return _instantiator(_prefab, _root.transform);
        }

        private void ResetInstance(GameObject instance)
        {
            instance.transform.SetParent(_root.transform);
        }

        private void DisposeInstance(GameObject instance)
        {
            UnityEngine.Object.Destroy(instance);
        }
    }

    internal class AutoInjectObjectPoolImpl<T> : AutoInjectObjectPoolImpl, IAutoInjectObjectPool<T> where T : Component
    {
        internal AutoInjectObjectPoolImpl(
                Transform root, T prefab, int minInstance, int maxInstance,
                Pin.Instantiator instantiator)
            : base(root, prefab.gameObject, minInstance, maxInstance, instantiator)
        { }

        public new T Spawn(Transform parent)
        {
            var instance = base.Spawn(parent);
            return instance.GetComponent<T>();
        }

        public new T Spawn(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true)
        {
            var instance = base.Spawn(position, rotation, parent, worldSpace);
            return instance.GetComponent<T>();
        }

        public void Despawn(T instance)
        {
            Despawn(instance.gameObject);
        }
    }
}
