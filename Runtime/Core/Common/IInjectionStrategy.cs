// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

namespace Cathei.PinInject.Internal
{
    public interface IInjectionStrategy<in T>
    {
        void Inject(T obj, IDependencyContainer container, Pin.ContextConfiguration config);
    }
}
