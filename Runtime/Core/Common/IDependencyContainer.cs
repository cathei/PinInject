// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject
{
    /// <summary>
    /// Container that holds and resolves dependencies.
    /// Can be a part of hierarchical context.
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Resolves and returns a dependency with given type and name.
        /// Returns null if failed to find dependency.
        /// </summary>
        object Resolve(Type type, string name);
    }

    public static class DependencyContainerExtensions
    {
        /// <summary>
        /// Resolves and returns a dependency with given type.
        /// Returns null if failed to find dependency.
        /// </summary>
        public static T Resolve<T>(this IDependencyContainer container)
        {
            return (T)container.Resolve(typeof(T), null);
        }

        /// <summary>
        /// Resolves and returns a dependency with given type and name.
        /// Returns null if failed to find dependency.
        /// </summary>
        public static T Resolve<T>(this IDependencyContainer container, string name)
        {
            return (T)container.Resolve(typeof(T), name);
        }
    }
}