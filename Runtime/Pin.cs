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
        private static readonly Dictionary<int, InjectContainerImpl> _sharedContainers = new Dictionary<int, InjectContainerImpl>();
        private static readonly Dictionary<int, InjectContainerImpl> _sceneContainers = new Dictionary<int, InjectContainerImpl>();

        private static readonly UnityInjectStrategy _injectStrategy = new UnityInjectStrategy();

        public static void Inject<TObject>(TObject obj, IInjectContainer container = null) where TObject : class
        {
            _injectStrategy.Inject(obj, container);
        }
    }
}
