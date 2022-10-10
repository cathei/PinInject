// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;

namespace Cathei.PinInject
{
    /// <summary>
    /// Represents a persistent composition root that persists on Application lifecycle.
    /// </summary>
    public class PersistentCompositionRoot : MonoBehaviour, ICompositionRoot
    {
        private void Awake()
        {
            if (gameObject.scene.name != "DontDestroyOnLoad")
                throw new InjectionException("SharedInjectRoot must not be in a scene");
        }
    }
}