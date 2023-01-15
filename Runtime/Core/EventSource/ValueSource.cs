// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    /// <summary>
    /// EventSource with state in value. Acts similar to ReactiveProperty.
    /// </summary>
    public class ValueSource<T> : EventSource<T>, IValueSource<T>
    {
        public T Value { get; private set; }

        public override event Action<T> OnNext
        {
            add
            {
                // provide current value on subscribing
                value?.Invoke(Value);
                base.OnNext += value;
            }
            remove
            {
                base.OnNext -= value;
            }
        }

        public override void Publish(T value)
        {
            Value = value;
            base.Publish(value);
        }
    }
}
