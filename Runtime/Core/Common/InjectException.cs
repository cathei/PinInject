using System;
using System.Collections;
using System.Collections.Generic;

namespace Cathei.PinInject
{
    public class InjectException : Exception
    {
        public InjectException() { }

        public InjectException(string message) : base(message) { }
    }
}