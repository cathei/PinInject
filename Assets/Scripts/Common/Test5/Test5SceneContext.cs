// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SceneContext : MonoBehaviour, IInjectContext, IPostInjectHandler
{
    [Inject]
    private List<string> _initOrder;

    public void Configure(IInjectBinder binder)
    {
        binder.Bind(new BindWithNew("Test Scene 5"));
    }

    public void PostInject()
    {
        _initOrder.Add(nameof(Test5SceneContext));
    }
}
