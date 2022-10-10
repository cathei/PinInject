// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    public interface IEventSource<out T>
    {
        event Action<T> Listeners;
    }
}
