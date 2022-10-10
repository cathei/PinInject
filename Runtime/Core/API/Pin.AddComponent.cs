// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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

            IDependencyContainer container;

            if (gameObject.TryGetComponent(out DependencyContainerComponent containerComponent))
            {
                container = containerComponent.Container;
            }
            else
            {
                container = gameObject.FindParentContainer();
            }

            // perform regular C# object injection
            Strategy.Inject(component, container);

            return component;
        }
    }
}
