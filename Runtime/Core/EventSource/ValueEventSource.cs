using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cathei.PinInject.UI
{
    public class ValueEventSource<T> : EventSource<T>
    {
        public T Value { get; private set; }

        public override event Action<T> Listeners
        {
            add
            {
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
