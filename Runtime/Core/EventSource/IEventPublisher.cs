// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

namespace Cathei.PinInject.UI
{
    public interface IEventPublisher<in T>
    {
        void Publish(T value);
    }
}
