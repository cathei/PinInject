// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to any GameObject that has IInjectContext MonoBehaviour.
    /// This will track the container, until the GameObject is injected again.
    /// </summary>
    public class InjectContainerComponent : MonoBehaviour
    {
        internal readonly InjectContainerImpl _container = new InjectContainerImpl();

        [SerializeField] internal InjectContainerComponent parent;
    }
}
