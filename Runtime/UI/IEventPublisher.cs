using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public interface IEventPublisher<T>
    {
        void Publish(T value);
    }

    public class EventPublisher<T> : IEventPublisher<T>, IEventSource<T>
    {
        public event Action<T> Listeners;

        public EventPublisher() { }

        public void Publish(T value)
        {
            Listeners?.Invoke(value);
        }
    }
}
