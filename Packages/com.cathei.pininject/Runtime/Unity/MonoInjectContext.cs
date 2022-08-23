using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public abstract class MonoInjectContext : MonoBehaviour, IInjectContext
    {
        private InjectContainer _container;

        public abstract void Configure(InjectContainer container);

        internal InjectContainer ConfigureInternal()
        {
            if (_container != null)
                return _container;

            _container = new InjectContainer();
            Configure(_container);
            return _container;
        }
    }
}
