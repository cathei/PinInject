// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    public class ValueEventSource<T> : EventSource<T>
    {
        public T Value { get; private set; }

        public override event Action<T> Listeners
        {
            add
            {
                // provide current value on subscribing
                value?.Invoke(Value);
                base.Listeners += value;
            }
            remove
            {
                base.Listeners -= value;
            }
        }

        public override void Publish(T value)
        {
            Value = value;
            base.Publish(value);
        }
    }
}
