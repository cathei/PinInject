// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public interface IStrategy
    {
        void Inject(object obj, IDependencyContainer container);
    }
}
