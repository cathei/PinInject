// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    internal class EventSourceSubscription<T> : IDisposable
    {
        private readonly IEventSource<T> _source;
        private readonly Action<T> _action;

        internal EventSourceSubscription(IEventSource<T> source, Action<T> action)
        {
            _source = source;
            _action = action;

            _source.OnNext += _action;
        }

        public void Dispose()
        {
            _source.OnNext -= _action;
        }
    }
}
