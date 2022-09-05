using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public class EventPublisherObserver<T> : IObserver<T>
    {
        private readonly IEventPublisher<T> _publisher;

        public EventPublisherObserver(IEventPublisher<T> publisher)
        {
            _publisher = publisher;
        }

        public void OnNext(T value)
        {
            _publisher.Publish(value);
        }

        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            throw error;
        }
    }

    public static class ObservableEventSourceExtensions
    {
        public static void Subscribe<T>(this IObservable<T> observable, IEventPublisher<T> publisher)
        {
            observable.Subscribe(new EventPublisherObserver<T>(publisher));
        }
    }
}
