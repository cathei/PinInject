using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public interface IEventPublisher<in T>
    {
        void Publish(T value);
    }
}
