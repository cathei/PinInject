using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cathei.PinInject.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        public static T AddComponent<T>(GameObject gameObject)
            where T : Component
        {
            return AddComponent(gameObject, typeof(T)) as T;
        }

        public static Component AddComponent(GameObject gameObject, Type componentType)
        {
            var component = gameObject.AddComponent(componentType);

            if (component == null)
                return null;

            IInjectContainer container;

            if (gameObject.TryGetComponent(out InjectContainerComponent containerComponent))
            {
                container = containerComponent._container;
            }
            else
            {
                container = _injectStrategy.FindParentContainer(gameObject.transform);
            }

            // perform regular C# object injection
            _injectStrategy.Inject(component, container);

            return component;
        }
    }
}
