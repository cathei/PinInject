using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to Prefab and cache innner reference to injectable Unity Object.
    /// </summary>
    public class InjectCacheComponent : MonoBehaviour
    {
        [Serializable]
        public struct InnerPrefabReferences
        {
            public UnityEngine.Object unityObject;
        }

        private List<InnerPrefabReferences> _innerReferences = new();
    }
}
