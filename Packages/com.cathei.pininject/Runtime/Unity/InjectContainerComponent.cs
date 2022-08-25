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
    public class InjectContainerComponent : MonoBehaviour
    {
        [Inject] internal InjectContainer _container;
    }
}
