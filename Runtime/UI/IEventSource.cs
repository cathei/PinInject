using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public interface IEventSource<T>
    {
        event Action<T> Listeners;
    }

    public class ObservableEventSource<T> : IEventSource<T>, IObserver<T>, IDisposable
    {
        public event Action<T> Listeners;

        private readonly IDisposable _subscription;

        public ObservableEventSource(IObservable<T> source)
        {
            _subscription = source.Subscribe(this);
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public void OnNext(T value)
        {
            Listeners?.Invoke(value);
        }

        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }
    }
}
