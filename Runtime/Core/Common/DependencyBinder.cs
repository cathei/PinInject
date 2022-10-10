// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public readonly ref struct DependencyBinder
    {
        private readonly DependencyContainer _container;

        public DependencyBinder(DependencyContainer container)
        {
            _container = container;
        }

        public void Bind<T>(T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(instance);
        }

        public void Bind<T>(string name, T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(name, instance);
        }
    }
}