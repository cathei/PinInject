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
        private static readonly List<IInjectContext> _sceneContexts = new List<IInjectContext>();

        internal static int _resetCount = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            // for editor check
            _resetCount++;

            _rootContainer.Reset();
            _sceneContainers.Clear();
        }

        internal static void SetUpScene(SceneInjectRoot injectRoot)
        {
            Scene scene = injectRoot.gameObject.scene;

            if (_sceneContainers.ContainsKey(scene.handle))
                throw new InjectException("Scene should only contain one SceneInjectRoot");

            var containerImpl = new InjectContainerImpl();
            containerImpl.SetParent(_rootContainer);

            // register scene container
            _sceneContainers.Add(scene.handle, containerImpl);

            injectRoot.GetComponentsInChildren(true, _sceneContexts);

            // injecting and configuring scene contexts
            foreach (var context in _sceneContexts)
                _injectStrategy.InjectBindReslove(context, containerImpl, containerImpl);

            scene.GetRootGameObjects(_sceneRootObjects);

            // inject all other game objects in scene
            for (int i = 0; i < _sceneRootObjects.Count; ++i)
            {
                GameObject rootObject = _sceneRootObjects[i];

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
                return _rootContainer;

            if (!_sceneContainers.TryGetValue(scene.handle, out var container))
                throw new InjectException("Scene is not loaded");

            return container;
        }
    }
}
