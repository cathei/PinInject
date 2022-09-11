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
            public InjectContainerComponent container;
            public MonoBehaviour component;
        }

        [SerializeField]
        private List<InnerPrefabReferences> _innerReferences = new List<InnerPrefabReferences>();

        public List<InnerPrefabReferences> InnerReferences => _innerReferences;

        [SerializeField]
        private int _resetCount = -1;

        public bool IsValid
        {
            get => _resetCount == Pin._resetCount;
            set
            {
                if (value)
                    _resetCount = Pin._resetCount;
                else
                    _resetCount = -1;
            }
        }
    }
}
