// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5Singleton : MonoBehaviour, IPostInjectHandler
{
    [Inject]
    private List<string> _initOrder;

    public void PostInject()
    {
        _initOrder.Add(nameof(Test5Singleton));
    }
}
