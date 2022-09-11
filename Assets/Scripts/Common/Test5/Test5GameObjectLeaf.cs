using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5GameObjectLeaf : MonoBehaviour, IPostInjectHandler
{
    [Inject]
    private List<string> _initOrder;

    [Inject]
    public Test5Singleton singleton;

    public void PostInject()
    {
        _initOrder.Add(nameof(Test5GameObjectLeaf));
    }
}
