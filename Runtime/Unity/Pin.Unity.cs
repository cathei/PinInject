using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static readonly List<GameObject> _sceneRootObjects = new List<GameObject>();
        private static readonly List<ISceneInjectContext> _sceneContexts = new List<ISceneInjectContext>();

        internal static int _resetCount = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            // for editor check
            _resetCount++;

            _rootContainer.Reset();
            _sceneContainers.Clear();

            // editor can have open scene when scene reload disabled
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                    continue;

                OnSceneLoaded(scene, LoadSceneMode.Single);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        internal struct PositionArgs
        {
            public Vector3 position;
            public Quaternion rotation;
            public bool worldSpace;

            public void Apply(Transform transform)
            {
                if (worldSpace)
                {
                    transform.position = position;
                    transform.rotation = rotation;
                }
                else
                {
                    transform.localPosition = position;
                    transform.localRotation = rotation;
                }
            }
        }

        public delegate GameObject InstantiatorDelegate(GameObject prefab, Transform parent);

        public static GameObject Instantiate(
            GameObject prefab, Transform parent = null, InstantiatorDelegate instantiator = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new PositionArgs
            {
                position = prefab.transform.position,
                rotation = prefab.transform.rotation,
                worldSpace = false
            }, instantiator);
        }

        public static GameObject Instantiate(
            GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true, InstantiatorDelegate instantiator = null)
        {
            instantiator ??= DefaultInstantiator;

            return InstantiateInternal(prefab, parent, new PositionArgs
            {
                position = position,
                rotation = rotation,
                worldSpace = worldSpace
            }, instantiator);
        }

        public static T Instantiate<T>(
            T prefab, Transform parent = null, InstantiatorDelegate instantiator = null) where T : Component
        {
            return Instantiate(prefab.gameObject, parent, instantiator).GetComponent<T>();
        }

        public static T Instantiate<T>(
            T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldSpace = true, InstantiatorDelegate instantiator = null)
            where T : Component
        {
            return Instantiate(prefab.gameObject, position, rotation, parent, worldSpace, instantiator).GetComponent<T>();
        }

        public static void Inject(GameObject obj)
        {
            _injectStrategy.Inject(obj, GetSceneContainer(obj.scene));
        }

        public static void Inject<TComponent>(TComponent obj)
            where TComponent : Component
        {
            Inject(obj.gameObject);
        }

        internal static GameObject DefaultInstantiator(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        private static GameObject InstantiateInternal(
            GameObject prefab, Transform parent, PositionArgs args, InstantiatorDelegate instantiator)
        {
            bool savedActiveSelf = prefab.activeSelf;

            try
            {
                // turn off prefab to make sure Awake() is not called before injection
                prefab.SetActive(false);

                // prefab cached components
                _injectStrategy.CacheInnerReferences(prefab);

                var instance = instantiator(prefab, parent);

                args.Apply(instance.transform);

                Inject(instance);

                instance.SetActive(savedActiveSelf);

                return instance;
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }
        }

        internal static InjectContainerImpl GetSceneContainer(Scene scene)
        {
            if (!scene.IsValid())
                return _rootContainer;

            if (!_sceneContainers.TryGetValue(scene.handle, out var container))
                throw new InjectException("Scene is not loaded");

            return container;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var container = new InjectContainerImpl();
            container.SetParent(_rootContainer);

            scene.GetRootGameObjects(_sceneRootObjects);

            // register all ISceneInjectContext
            for (int i = 0; i < _sceneRootObjects.Count; ++i)
            {
                var rootObject = _sceneRootObjects[i];

                rootObject.GetComponentsInChildren(true, _sceneContexts);

                for (int j = 0; j < _sceneContexts.Count; ++j)
                {
                    var context = _sceneContexts[j];
                    context.Configure(container);
                }
            }

            // register scene container
            _sceneContainers.Add(scene.handle, container);

            // inject all game objects
            for (int i = 0; i < _sceneRootObjects.Count; ++i)
            {
                var rootObject = _sceneRootObjects[i];

                Pin.Inject(rootObject);
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            _sceneContainers.Remove(scene.handle);
        }
    }
}
