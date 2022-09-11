using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    [DefaultExecutionOrder(-1000000000)]
    public class SceneInjectRoot : MonoBehaviour, IInjectRoot
    {
        [SerializeField]
        internal SharedInjectRoot sharedRoot;

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