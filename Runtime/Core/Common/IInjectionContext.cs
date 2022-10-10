// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

namespace Cathei.PinInject
{
    /// <summary>
    /// Represents a context that provides dependency.
    /// It can be nested by hierarchy.
    /// </summary>
    public interface IInjectionContext
    {
        void Configure(DependencyBinder binder);
    }
}