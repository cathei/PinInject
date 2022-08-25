using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static readonly InjectContainer _rootContainer = new();

        public static void ConfigureGlobal<T>() where T : IInjectContext, new()
        {
            new T().Configure(_rootContainer);
        }

        public static void ConfigureGlobal(IInjectContext context)
        {
            context.Configure(_rootContainer);
        }
    }
}
