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

            if (_sharedContainers.TryGetValue(instanceId, out var container))
                return container;

            var prefab = injectRoot.gameObject;

            bool savedActiveSelf = injectRoot.gameObject.activeSelf;

            try
            {
                prefab.SetActive(false);

                var instance = UnityEngine.Object.Instantiate(prefab, null);

                UnityEngine.Object.DontDestroyOnLoad(instance);

                // inject root first
                _injectStrategy.Inject(instance, null);

                var sharedContainer = _injectStrategy.GetContainerComponent(instance.gameObject);

                // register shared container
                _sharedContainers.Add(instanceId, sharedContainer._container);

                instance.SetActive(savedActiveSelf);

                return sharedContainer._container;
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }
        }

        internal static void SetUpScene(SceneInjectRoot injectRoot)
        {
            Scene scene = injectRoot.gameObject.scene;

            if (_sceneContainers.ContainsKey(scene.handle))
                throw new InjectException("Scene should only contain one SceneInjectRoot");

            var sharedContainer = SetUpShared(injectRoot.sharedRoot);

            // inject scene first
            _injectStrategy.Inject(injectRoot.gameObject, sharedContainer);

            var sceneContainer = _injectStrategy.GetContainerComponent(injectRoot.gameObject);

            // register scene container
            _sceneContainers.Add(scene.handle, sceneContainer._container);

            scene.GetRootGameObjects(_tempRootObjects);

            // inject all other game objects in scene
            for (int i = 0; i < _tempRootObjects.Count; ++i)
            {
                GameObject rootObject = _tempRootObjects[i];

                if (rootObject == injectRoot.gameObject)
                    continue;

                Pin.Inject(rootObject);
            }
        }

        internal static void TearDownScene(SceneInjectRoot injectRoot)
        {
            _sceneContainers.Remove(injectRoot.gameObject.scene.handle);
        }

        internal static InjectContainerImpl GetSceneContainer(Scene scene)
        {
            if (!scene.IsValid() || scene.buildIndex == -1)
                return null;

            if (!_sceneContainers.TryGetValue(scene.handle, out var container))
                throw new InjectException("There is no SceneInjectRoot in the scene");

            return container;
        }
    }
}
