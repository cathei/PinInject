using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public class InjectContainer
    {
        // type -> constructor

        // type -> instance

        // direct parent to current container
        private InjectContainer _parent;

        internal void SetParent(InjectContainer parent)
        {
            _parent = parent;
        }

        internal void Reslove<T>()
        {

        }
    }
}