// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;

namespace Cathei.PinInject
{
    public interface IDependencyContainer
    {
        object Resolve(Type type, string id);
    }

    public static class DependencyContainerExtensions
    {
        public static T Resolve<T>(this IDependencyContainer container)
        {
            return (T)container.Resolve(typeof(T), null);
        }

        public static T Resolve<T>(this IDependencyContainer container, string id)
        {
            return (T)container.Resolve(typeof(T), id);
        }
    }
}