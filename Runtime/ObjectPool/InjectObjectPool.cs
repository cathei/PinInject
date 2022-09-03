using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Cathei.PinInject
{
    /// <summary>
    /// game object pool that injects on instantiation
    /// </summary>
    public class InjectObjectPool
    {
        private class PoolImpl : GenericObjectPool<GameObject>
        {
            internal readonly Transform _root;
            internal readonly GameObject _prefab;
            internal readonly Pin.InstantiatorDelegate _instantiator;

            public PoolImpl(Transform root, GameObject prefab, int minInstance, int maxInstance, Pin.InstantiatorDelegate instantiator)
                : base(minInstance, maxInstance)
            {
                _root = root;
                _prefab = prefab;
                _instantiator = instantiator;

                _root.gameObject.SetActive(false);
            }

            protected override GameObject CreateInstance()
            {
                return _instantiator(_prefab, _root.transform);
            }

            protected override void ResetInstance(GameObject instance)
            {
                instance.transform.SetParent(_root.transform);
            }

            protected override void DisposeInstance(GameObject instance)
            {
                UnityEngine.Object.Destroy(instance);
            }
        }

        private readonly PoolImpl _poolImpl;

        public InjectObjectPool(Transform root, GameObject prefab, int minInstance = 0, int maxInstance = 100, Pin.InstantiatorDelegate instantiator = null)
        {
            instantiator ??= Pin.DefaultInstantiator;
            _poolImpl = new PoolImpl(root, prefab, minInstance, maxInstance, instantiator);
        }

        public GameObject Instantiate(Transform parent)
        {
            return InstantiateInternal(parent, new Pin.PositionArgs
            {
                position = _poolImpl._prefab.transform.position,
                rotation = _poolImpl._prefab.transform.rotation,
                worldSpace = false
            });
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true)
        {
            return InstantiateInternal(parent, new Pin.PositionArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            });
        }

        private GameObject InstantiateInternal(Transform parent, Pin.PositionArgs args)
        {
            var instance = _poolImpl.Get();

            instance.SetActive(false);

            instance.transform.SetParent(parent);

            args.Apply(instance.transform);

            Pin.Inject(instance);

            instance.SetActive(true);
            return instance;
        }

        public void Destroy(GameObject instance)
        {
            _poolImpl.Release(instance);
        }
    }

    public class InjectObjectPool<T> : InjectObjectPool where T : Component
    {
        public InjectObjectPool(Transform root, T prefab, int minInstance = 0, int maxInstance = 100)
            : base(root, prefab.gameObject, minInstance, maxInstance)
        { }

        public new T Instantiate(Transform parent)
        {
            var instance = base.Instantiate(parent);
            return instance.GetComponent<T>();
        }

        public new T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true)
        {
            var instance = base.Instantiate(position, rotation, parent, worldSpace);
            return instance.GetComponent<T>();
        }

        public void Destroy(T instance)
        {
            Destroy(instance.gameObject);
        }
    }
}
