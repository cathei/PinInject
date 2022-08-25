using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to any GameObject that has IInjectContext MonoBehaviour.
    /// This will keep the container, until the GameObject is injected again.
    /// </summary>
    public class InjectContextComponent : MonoBehaviour
    {
        private InjectContainer _container;

        // internal InjectContainer ConfigureInternal()
        // {
        //     if (_container != null)
        //         return _container;

        //     _container = new InjectContainer();
        //     Configure(_container);
        //     return _container;
        // }
    }
}
