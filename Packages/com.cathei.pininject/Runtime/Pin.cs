using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static List<IInjectContext> _globalContexts = new List<IInjectContext>();

        public static void AddGlobalContext(IInjectContext context)
        {
            _globalContexts.Add(context);
        }
    }
    
}