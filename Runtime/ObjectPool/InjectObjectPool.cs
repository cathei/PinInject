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
    public class InjectObjectPool<T> where T : UnityEngine.Object, new()
    {
        private class PoolImpl : GenericObjectPool<T>
        {
            private readonly GameObject _root;
            private readonly T _prefab;

            public PoolImpl(GameObject root, T prefab, int minInstance, int maxInstance)
                : base(minInstance, maxInstance)
            {
                _root = root;
                _prefab = prefab;

                _root.SetActive(false);
            }

            protected override T CreateInstance()
            {
                return UnityEngine.Object.Instantiate(_prefab, _root.transform);
            }

            protected override void ResetInstance(T instance)
            {
                if (instance is Component component)
                    component.transform.SetParent(_root.transform);
                else if (instance is GameObject gameObject)
                    gameObject.transform.SetParent(_root.transform);
            }

            protected override void DisposeInstance(T instance)
            {
                if (instance is Component component)
                    UnityEngine.Object.Destroy(component.gameObject);
                else
                    UnityEngine.Object.Destroy(instance);
            }
        }

        private readonly GenericObjectPool<T> _poolImpl;

        public InjectObjectPool(GameObject root, T prefab, int minInstance = 0, int maxInstance = 100)
        {
            _poolImpl = new PoolImpl(root, prefab, minInstance, maxInstance);
        }

        public T Instantiate(Transform parent, Vector3 position, Quaternion rotation)
        {
            var instance = _poolImpl.Get();

            GameObject gameObject;

            if (instance is Component component)
                gameObject = component.gameObject;
            else if (instance is GameObject go)
                gameObject = go;
            else
                throw new InvalidOperationException("Type must be Component or GameObject");

            gameObject.SetActive(false);

            gameObject.transform.SetParent(parent);

            UnityEngine.Object.Instantiate(gameObject, parent);

            Pin.Inject(gameObject);

            gameObject.SetActive(true);
            return instance;
        }

        public void Destroy(T instance)
        {
            _poolImpl.Release(instance);
        }
    }
}
