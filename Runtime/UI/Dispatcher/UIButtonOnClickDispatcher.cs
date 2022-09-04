using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonOnClickDispatcher : UIDispatcherBase<Button, object>
    {
        protected override void Register(IEventPublisher<object> publisher)
        {
            target.onClick.AddListener(() => publisher.Publish(default));
        }
    }
}
