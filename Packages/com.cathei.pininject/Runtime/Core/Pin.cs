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
        private static readonly InjectContainerImpl _rootContainer = new InjectContainerImpl();

        private static readonly Dictionary<int, InjectContainerImpl> _sceneContainers = new Dictionary<int, InjectContainerImpl>();

        private static readonly UnityInjectStrategy _injectStrategy = new UnityInjectStrategy();

        public static void AddGlobalContext<TContext>() where TContext : IInjectContext, new()
        {
            new TContext().Configure(_rootContainer);
        }

        public static void AddGlobalContext(IInjectContext context)
        {
            context.Configure(_rootContainer);
        }

        public static void Inject<TObject>(TObject obj, IInjectContainer container = null) where TObject : class
        {
            container ??= _rootContainer;
            _injectStrategy.Inject(obj, container);
        }
    }
}
