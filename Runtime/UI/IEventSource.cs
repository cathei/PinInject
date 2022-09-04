using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public interface IEventSource<out T>
    {
        event Action<T> Listeners;
    }
}
