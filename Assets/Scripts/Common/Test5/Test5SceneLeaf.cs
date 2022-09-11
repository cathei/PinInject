using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SceneLeaf : MonoBehaviour, IPostInjectHandler
{
    [Inject]
    private List<string> _initOrder;

    public void PostInject()
    {
        _initOrder.Add(nameof(Test5SceneLeaf));
    }
}
