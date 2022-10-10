// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cathei.PinInject
{
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