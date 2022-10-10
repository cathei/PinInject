// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

namespace Cathei.PinInject
{
    public interface IInjectionContext
    {
        void Configure(DependencyBinder binder);
    }
}