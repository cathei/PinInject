// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonOnClickDispatcher : UIDispatcherBase<Button, object>
    {
        protected override void Register(IEventPublisher<object> publisher)
        {
            target.onClick.AddListener(() => publisher.Publish(default));
        }
    }
}
