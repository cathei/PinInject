// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static readonly IStrategy Strategy = new UnityStrategy();

        public static void Inject<TObject>(TObject obj, IDependencyContainer container = null) where TObject : class
        {
            Strategy.Inject(obj, container);
        }
    }
}
