using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public interface IInjectContainer
    {
        object Resolve(Type type, string id);
    }
}