// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject.UI
{
    public interface IValueSource<out T> : IEventSource<T>
    {
        T Value { get; }
    }
}
