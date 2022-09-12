// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public interface IInjectContext
    {
        void Configure(IInjectBinder binder);
    }
}