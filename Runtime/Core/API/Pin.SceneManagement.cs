// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static readonly Dictionary<int, DependencyContainer> _persistentContainers = new();
        private static readonly Dictionary<int, DependencyContainer> _sceneContainers = new();

        private static readonly List<GameObject> _tempRootObjects = new();

        internal static int _resetCount = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            // for editor check
            _resetCount++;

            _persistentContainers.Clear();
            _sceneContainers.Clear();
        }

        internal static IDependencyContainer SetUpPersistent(PersistentCompositionRoot compositionRoot)
        {
            if (compositionRoot == null)
                return null;

            int instanceId = compositionRoot.GetInstanceID();

            if (_persistentContainers.TryGetValue(instanceId, out var container))
                return container;

            var prefab = compositionRoot.gameObject;
            bool savedActiveSelf = prefab.activeSelf;

            try
            {
                prefab.SetActive(false);

                var instance = Object.Instantiate(prefab, null);

                Object.DontDestroyOnLoad(instance);

                // inject root first
                UnityStrategy.Inject(instance, null, null);

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
            var compositionRootObject = compositionRoot.gameObject;
            var scene = compositionRootObject.scene;

            if (_sceneContainers.ContainsKey(scene.handle))
                throw new InjectionException("Scene should only contain one SceneInjectRoot");

            var persistentContainer = SetUpPersistent(compositionRoot.parent);

            // inject scene first
            UnityStrategy.Inject(compositionRootObject, persistentContainer, null);

            var sceneContainer = compositionRootObject.GetOrAddContainerComponent();

            // register scene container
            _sceneContainers.Add(scene.handle, sceneContainer.Container);

            scene.GetRootGameObjects(_tempRootObjects);

            // inject all other game objects in scene
            for (int i = 0; i < _tempRootObjects.Count; ++i)
            {
                var rootObject = _tempRootObjects[i];

                // inject except scene composition root
                // childed case will be filtered in Inject
                if (rootObject == compositionRootObject)
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
