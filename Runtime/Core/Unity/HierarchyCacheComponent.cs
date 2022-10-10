// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
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
        public struct Node
        {
            public DependencyContainerComponent container;
            public DependencyContainerComponent parent;
            public List<MonoBehaviour> components;
        }

        [SerializeField]
        private List<Node> innerReferences = new List<Node>();

        [SerializeField]
        private int resetCount = -1;

        public List<Node> InnerReferences => innerReferences;

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
