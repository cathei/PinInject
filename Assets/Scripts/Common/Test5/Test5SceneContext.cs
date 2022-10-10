// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SceneContext : MonoBehaviour, IInjectionContext, IPostInjectHandler
{
    [Inject]
    private List<string> _initOrder;

    public void Configure(DependencyBinder binder)
    {
        binder.Bind(new BindWithNew("Test Scene 5"));
    }

    public void PostInject()
    {
        _initOrder.Add(nameof(Test5SceneContext));
    }
}
