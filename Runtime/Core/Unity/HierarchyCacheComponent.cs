// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject.Internal
{
    /// <summary>
    /// This component will be attached to Prefab and cache innner reference to injectable Unity Object.
    /// </summary>
    public class HierarchyCacheComponent : MonoBehaviour
    {
        [Serializable]
        public struct InnerPrefabReferences
        {
            public DependencyContainerComponent container;
            public MonoBehaviour component;
        }

        [SerializeField]
        private List<InnerPrefabReferences> innerReferences = new List<InnerPrefabReferences>();

        [SerializeField]
        private int resetCount = -1;

        public List<InnerPrefabReferences> InnerReferences => innerReferences;

        public bool IsValid
        {
            get => resetCount == Pin._resetCount;
            set
            {
                if (value)
                    resetCount = Pin._resetCount;
                else
                    resetCount = -1;
            }
        }
    }
}
