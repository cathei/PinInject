// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    public class EventSource<T> : IEventPublisher<T>, IEventSource<T>
    {
        public virtual event Action<T> Listeners;

        public EventSource() { }

        public virtual void Publish(T value)
        {
            Listeners?.Invoke(value);
        }
    }

    public static class EventSourceExtensions
    {
        public static void BindEventSource<T>(this DependencyBinder binder, string name, EventSource<T> source)
        {
            binder.Bind<IEventSource<T>>(name, source);
            binder.Bind<IEventPublisher<T>>(name, source);
        }
    }
}
