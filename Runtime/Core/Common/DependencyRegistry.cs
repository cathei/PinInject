// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public readonly ref struct DependencyRegistry
    {
        private readonly DependencyContainer _container;

        public DependencyRegistry(DependencyContainer container)
        {
            _container = container;
        }

        public void Add<T>(T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Add(instance);
        }

        public void Add<T>(string name, T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Add(name, instance);
        }
    }
}