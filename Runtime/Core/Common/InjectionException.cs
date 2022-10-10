// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject
{
    public class InjectionException : Exception
    {
        public InjectionException() { }

        public InjectionException(string message) : base(message) { }
    }
}