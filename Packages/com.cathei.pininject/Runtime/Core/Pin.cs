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
        private static readonly InjectContainer _rootContainer = new InjectContainer();

        private static readonly Dictionary<Scene, InjectContainer> _sceneContainers = new Dictionary<Scene, InjectContainer>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            _rootContainer.Reset();
            _sceneContainers.Clear();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public static void AddGlobalContext<T>() where T : IInjectContext, new()
        {
            new T().Configure(_rootContainer);
        }

        public static void AddGlobalContext(IInjectContext context)
        {
            context.Configure(_rootContainer);
        }

        public static void Inject<T>(T obj, InjectContainer container = null) where T : class
        {
            container ??= _rootContainer;

            var cache = ReflectionCache.Get(obj.GetType());
            cache.Inject(obj, container);
        }

        internal static InjectContainer GetSceneContainer(Scene scene)
        {
            if (scene == null)
                return _rootContainer;

            if (!_sceneContainers.TryGetValue(scene, out var container))
                throw new InjectException("Scene is not loaded");

            return container;
        }

        private static readonly List<GameObject> _sceneRootObjects = new List<GameObject>();
        private static readonly List<ISceneInjectContext> _sceneContexts = new List<ISceneInjectContext>();

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var container = new InjectContainer();
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
            _sceneContainers.Add(scene, container);

            // inject all game objects
            for (int i = 0; i < _sceneRootObjects.Count; ++i)
            {
                var rootObject = _sceneRootObjects[i];

                Pin.InjectInternal(rootObject);
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            _sceneContainers.Remove(scene);
        }
    }
}
