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
        private static readonly List<GameObject> _tempRootObjects = new List<GameObject>();
        private static readonly List<IInjectContext> _tempContexts = new List<IInjectContext>();

        internal static int _resetCount = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            // for editor check
            _resetCount++;

            _sharedContainers.Clear();
            _sceneContainers.Clear();
        }

        internal static IInjectContainer SetUpShared(SharedInjectRoot injectRoot)
        {
            if (injectRoot == null)
                return null;

            int instanceId = injectRoot.GetInstanceID();

            if (_sharedContainers.TryGetValue(instanceId, out var containerImpl))
                return containerImpl;

            containerImpl = new InjectContainerImpl();
            _sharedContainers[instanceId] = containerImpl;

            var prefab = injectRoot.gameObject;

            bool savedActiveSelf = injectRoot.gameObject.activeSelf;

            try
            {
                prefab.SetActive(false);

                var instance = UnityEngine.Object.Instantiate(prefab, null);

                UnityEngine.Object.DontDestroyOnLoad(instance);

                injectRoot.GetComponentsInChildren(true, _tempContexts);

                // injecting and configuring shared contexts
                foreach (var context in _tempContexts)
                    _injectStrategy.InjectBindResolve(context, containerImpl, containerImpl);
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }

            return containerImpl;
        }

        internal static void SetUpScene(SceneInjectRoot injectRoot)
        {
            Scene scene = injectRoot.gameObject.scene;

            if (_sceneContainers.ContainsKey(scene.handle))
                throw new InjectException("Scene should only contain one SceneInjectRoot");

            var sharedContainer = SetUpShared(injectRoot.sharedRoot);

            var containerImpl = new InjectContainerImpl();
            containerImpl.SetParent(sharedContainer);

            // register scene container
            _sceneContainers.Add(scene.handle, containerImpl);

            injectRoot.GetComponentsInChildren(true, _tempContexts);

            // injecting and configuring scene contexts
            foreach (var context in _tempContexts)
                _injectStrategy.InjectBindResolve(context, containerImpl, containerImpl);

            scene.GetRootGameObjects(_tempRootObjects);

            // inject all other game objects in scene
            for (int i = 0; i < _tempRootObjects.Count; ++i)
            {
                GameObject rootObject = _tempRootObjects[i];

                Pin.Inject(rootObject);
            }
        }

        internal static void TearDownScene(SceneInjectRoot injectRoot)
        {
            _sceneContainers.Remove(injectRoot.gameObject.scene.handle);
        }

        internal static InjectContainerImpl GetSceneContainer(Scene scene)
        {
            if (!scene.IsValid())
                return null;

            if (!_sceneContainers.TryGetValue(scene.handle, out var container))
                throw new InjectException("Scene is not loaded");

            return container;
        }
    }
}
