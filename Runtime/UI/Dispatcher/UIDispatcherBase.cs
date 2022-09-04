using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    public abstract class UIDispatcherBase<TComponent, TParam> : MonoBehaviour
    {
        public string identifier;

        protected TComponent target;

        [Inject(nameof(identifier), true)]
        private IEventPublisher<TParam> _eventPublisher;

        protected virtual void OnEnable()
        {
            target = GetComponent<TComponent>();
            Register(_eventPublisher);
        }

        protected abstract void Register(IEventPublisher<TParam> publisher);
    }
}
