// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SharedContext : MonoBehaviour, IInjectionContext, IPostInjectHandler
{
    public Test5Singleton singleton;

    public List<string> initOrder = new List<string>();

    public void Configure(DependencyBinder binder)
    {
        binder.Bind(singleton);
        binder.Bind(initOrder);
    }

    public void PostInject()
    {
        initOrder.Add(nameof(Test5SharedContext));
    }
}
