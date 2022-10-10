// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        /// <summary>
        /// Add component to GameObject, then inject to the component.
        /// Note that injection will happen after `Awake` and `OnEnable`.
        /// </summary>
        public static T AddComponent<T>(GameObject gameObject)
            where T : Component
        {
            return AddComponent(gameObject, typeof(T)) as T;
        }

        /// <summary>
        /// Add component to GameObject, then inject to the component.
        /// Note that injection will happen after `Awake` and `OnEnable`.
        /// </summary>
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
            DefaultStrategy.Inject(component, container, null);

            return component;
        }
    }
}
