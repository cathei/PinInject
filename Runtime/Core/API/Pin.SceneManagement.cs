// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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
        private static readonly Dictionary<int, DependencyContainer> _persistentContainers = new Dictionary<int, DependencyContainer>();
        private static readonly Dictionary<int, DependencyContainer> _sceneContainers = new Dictionary<int, DependencyContainer>();

        private static readonly List<GameObject> _tempRootObjects = new List<GameObject>();
        private static readonly List<IContext> _tempContexts = new List<IContext>();

        internal static int _resetCount = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            // for editor check
            _resetCount++;

            _persistentContainers.Clear();
            _sceneContainers.Clear();
        }

        internal static IDependencyContainer SetUpShared(PersistentCompositionRoot compositionRoot)
        {
            if (compositionRoot == null)
                return null;

            int instanceId = compositionRoot.GetInstanceID();

            if (_persistentContainers.TryGetValue(instanceId, out var container))
                return container;

            var prefab = compositionRoot.gameObject;

            bool savedActiveSelf = compositionRoot.gameObject.activeSelf;

            try
            {
                prefab.SetActive(false);

                var instance = UnityEngine.Object.Instantiate(prefab, null);

                UnityEngine.Object.DontDestroyOnLoad(instance);

                // inject root first
                Strategy.Inject(instance, null);

                var persistentContainer = instance.gameObject.GetOrAddContainerComponent();

                // register persistent container
                _persistentContainers.Add(instanceId, persistentContainer.Container);

                instance.SetActive(savedActiveSelf);

                return persistentContainer.Container;
            }
            finally
            {
                prefab.SetActive(savedActiveSelf);
            }
        }

        internal static void SetUpScene(SceneCompositionRoot compositionRoot)
        {
            Scene scene = compositionRoot.gameObject.scene;

            if (_sceneContainers.ContainsKey(scene.handle))
                throw new InjectionException("Scene should only contain one SceneInjectRoot");

            var sharedContainer = SetUpShared(compositionRoot.parent);

            // inject scene first
            Strategy.Inject(compositionRoot.gameObject, sharedContainer);

            var sceneContainer = compositionRoot.gameObject.GetOrAddContainerComponent();

            // register scene container
            _sceneContainers.Add(scene.handle, sceneContainer.Container);

            scene.GetRootGameObjects(_tempRootObjects);

            // inject all other game objects in scene
            for (int i = 0; i < _tempRootObjects.Count; ++i)
            {
                GameObject rootObject = _tempRootObjects[i];

                if (rootObject == compositionRoot.gameObject)
                    continue;

                Inject(rootObject);
            }
        }

        internal static void TearDownScene(SceneCompositionRoot compositionRoot)
        {
            _sceneContainers.Remove(compositionRoot.gameObject.scene.handle);
        }

        internal static DependencyContainer GetSceneContainer(Scene scene)
        {
            if (!scene.IsValid() || scene.buildIndex == -1)
                return null;

            if (!_sceneContainers.TryGetValue(scene.handle, out var container))
                throw new InjectionException("There is no SceneInjectRoot in the scene");

            return container;
        }
    }
}
