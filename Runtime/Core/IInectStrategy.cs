using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject.Internal
{
    public interface IInjectStrategy
    {
        void Inject(object obj, IInjectContainer container);
    }
}
