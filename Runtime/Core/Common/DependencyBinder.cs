// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;

namespace Cathei.PinInject
{
    /// <summary>
    /// Dependency binder used for configuration of IInjectionContext.
    /// </summary>
    public readonly ref struct DependencyBinder
    {
        private readonly DependencyContainer _container;

        public IDependencyContainer Container => _container;

        public DependencyBinder(DependencyContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Bind an object instance as a given type.
        /// Manually specify generic argument to use interface type.
        /// </summary>
        public void Bind<T>(T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(typeof(T), null, instance);
        }

        /// <summary>
        /// Bind an object instance as a given type and name.
        /// Manually specify generic argument to use interface type.
        /// </summary>
        public void Bind<T>(string name, T instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(typeof(T), name, instance);
        }

        /// <summary>
        /// Bind an object instance as a given type.
        /// </summary>
        public void Bind(Type type, object instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(type, null, instance);
        }

        /// <summary>
        /// Bind an object instance as a given type and name.
        /// </summary>
        public void Bind(Type type, string name, object instance)
        {
            if (_container == null)
                throw new InjectionException("Binding failed: no container assigned!");

            _container.Bind(type, name, instance);
        }
    }
}