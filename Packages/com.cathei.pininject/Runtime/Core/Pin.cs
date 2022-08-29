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

        private static readonly UnityInjectStrategy _injectStrategy = new UnityInjectStrategy();

        public static void AddGlobalContext<TContext>() where TContext : IInjectContext, new()
        {
            new TContext().Configure(_rootContainer);
        }

        public static void AddGlobalContext(IInjectContext context)
        {
            context.Configure(_rootContainer);
        }

        public static void Inject<TObject>(TObject obj, InjectContainer container = null) where TObject : class
        {
            container ??= _rootContainer;
            _injectStrategy.Inject(obj, container);
        }
    }
}
