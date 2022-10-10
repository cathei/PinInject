// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        private static readonly IStrategy Strategy = new UnityStrategy();

        public delegate void ContextConfiguration(DependencyBinder binder);

        public static void Inject<TObject>(TObject obj, ContextConfiguration config)
            where TObject : class
        {
            Inject(obj, null, config);
        }

        public static void Inject<TObject>(
                TObject obj, IDependencyContainer container = null, ContextConfiguration config = null)
            where TObject : class
        {
            object target = obj;

            if (obj is GameObject gameObject)
            {
                target = gameObject;

                Debug.Assert(container == null,
                    "GameObject must not specify container manually!");

                container = GetSceneContainer(gameObject.scene);

            }
            else if (obj is Component component)
            {
                target = component.gameObject;

                Debug.Assert(container == null,
                    "Component must not specify container manually!");

                container = GetSceneContainer(component.gameObject.scene);
            }

            InjectInternal(target, container, config);
        }

        private static void InjectInternal<TObject>(
                TObject obj, IDependencyContainer container, ContextConfiguration config)
            where TObject : class
        {
            if (config != null)
            {
                var localContainer = new DependencyContainer(container);
                config(new DependencyBinder(localContainer));
                container = localContainer;
            }

            Strategy.Inject(obj, container);
        }
    }
}
