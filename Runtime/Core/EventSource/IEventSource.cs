// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    public interface IEventSource<out T> : IObservable<T>
    {
        event Action<T> Listeners;

        IDisposable Subscribe(Action<T> action);
    }
}
