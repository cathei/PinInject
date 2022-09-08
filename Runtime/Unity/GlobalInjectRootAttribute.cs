using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class GlobalInjectRootAttribute : RuntimeInitializeOnLoadMethodAttribute
    {
        public GlobalInjectRootAttribute() : base(RuntimeInitializeLoadType.BeforeSplashScreen) { }
    }
}
