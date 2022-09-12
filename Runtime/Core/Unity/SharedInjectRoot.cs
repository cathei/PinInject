// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cathei.PinInject
{
    public class SharedInjectRoot : MonoBehaviour, IInjectRoot
    {
        private void Awake()
        {
            if (gameObject.scene.name != "DontDestroyOnLoad")
                throw new InjectException("SharedInjectRoot must not be in a scene");
        }
    }
}