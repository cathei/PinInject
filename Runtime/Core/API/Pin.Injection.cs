// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject.Internal;
using UnityEngine;

namespace Cathei.PinInject
{
    public static partial class Pin
    {
        internal static readonly DefaultInjectionStrategy DefaultStrategy = new();
        internal static readonly IInjectionStrategy<GameObject> UnityStrategy = new UnityInjectionStrategy();

        public delegate void ContextConfiguration(DependencyBinder binder);

        /// <summary>
        /// Inject an object. If it's GameObject or Component, all children will be injected as well.
        /// </summary>
        public static void Inject<TObject>(TObject obj, ContextConfiguration config)
            where TObject : class
        {
            Inject(obj, null, config);
        }

        /// <summary>
        /// Inject an object. If it's GameObject or Component, all children will be injected as well.
        /// </summary>
        public static void Inject<TObject>(
                TObject obj, IDependencyContainer container = null, ContextConfiguration config = null)
            where TObject : class
        {
            if (obj is GameObject gameObject)
            {
                Debug.Assert(container == null,
                    "GameObject must not specify container manually!");

                InjectGameObjectInternal(gameObject, config);
                return;
            }

            if (obj is Component component)
            {
                Debug.Assert(container == null,
                    "Component must not specify container manually!");

                InjectGameObjectInternal(component.gameObject, config);
                return;
            }

            InjectInternal(obj, container, config);
        }

        private static void InjectInternal<TObject>(
                TObject obj, IDependencyContainer container, ContextConfiguration config)
            where TObject : class
        {
            DefaultStrategy.Inject(obj, container, config);
        }

        private static void InjectGameObjectInternal(GameObject gameObject, ContextConfiguration config)
        {
            var container = gameObject.FindParentContainer();
            UnityStrategy.Inject(gameObject, container, config);
        }
    }
}
