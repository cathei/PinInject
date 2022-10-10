// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    public delegate void ContextConfigureDelegate(DependencyRegistry registry);

    public readonly struct MethodContext : IContext
    {
        private readonly ContextConfigureDelegate _configureMethod;

        public MethodContext(ContextConfigureDelegate configureMethod)
        {
            _configureMethod = configureMethod;
        }

        public void Configure(DependencyRegistry registry)
        {
            _configureMethod(registry);
        }
    }
}
