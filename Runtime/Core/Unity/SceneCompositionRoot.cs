// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using UnityEngine;
using UnityEngine.Serialization;

namespace Cathei.PinInject
{
    /// <summary>
    /// Represents a composition root of a Scene context.
    /// Only one SceneCompositionRoot can exist on one scene.
    /// </summary>
    [DefaultExecutionOrder(-1000000000)]
    public class SceneCompositionRoot : MonoBehaviour, ICompositionRoot
    {
        [SerializeField, FormerlySerializedAs("sharedRoot")]
        public PersistentCompositionRoot parent;

        private void Awake()
        {
            Pin.SetUpScene(this);
        }

        private void OnDestroy()
        {
            Pin.TearDownScene(this);
        }
    }
}