using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to any GameObject that has IInjectContext MonoBehaviour.
    /// This will keep the container, until the GameObject is injected again.
    /// </summary>
    public class InjectContainerComponent : MonoBehaviour
    {
        [Inject] internal InjectContainer _container = new InjectContainer();

        public static InjectContainerComponent GetOrCreate(GameObject gameObject)
        {
            var component = gameObject.GetComponent<InjectContainerComponent>();
            if (component != null)
                return component;

            component = gameObject.AddComponent<InjectContainerComponent>();
            component.hideFlags = HideFlags.HideAndDontSave;
            return component;
        }

        public static InjectContainer FindParentContainer(Transform transform)
        {
            InjectContainer parentContainer = null;

            while (transform != null)
            {
                var component = transform.GetComponent<InjectContainerComponent>();

                if (component != null)
                {
                    parentContainer = component._container;
                    break;
                }

                transform = transform.parent;
            }

            if (parentContainer == null)
                parentContainer = Pin.GetSceneContainer(transform.gameObject.scene);

            return parentContainer;
        }
    }
}
