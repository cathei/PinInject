using UnityEngine;
using UnityEngine.UI;

namespace Cathei.PinInject.UI
{
    public abstract class UIBinderBase<TComponent, TParam> : MonoBehaviour
        where TComponent : Component
    {
        public string identifier;

        protected TComponent target;

        [Inject(nameof(identifier), true)]
        private IEventSource<TParam> _eventSource;

        protected virtual void Awake()
        {
            target = GetComponent<TComponent>();
        }

        protected virtual void OnEnable()
        {
            _eventSource.Listeners += HandleEvent;
        }

        protected virtual void OnDisable()
        {
            _eventSource.Listeners -= HandleEvent;
        }

        protected abstract void HandleEvent(TParam param);
    }
}
