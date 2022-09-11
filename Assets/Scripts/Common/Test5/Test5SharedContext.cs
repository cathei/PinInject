using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SharedContext : MonoBehaviour, IInjectContext, IPostInjectHandler
{
    public Test5Singleton singleton;

    public List<string> initOrder = new List<string>();

    public void Configure(IInjectBinder binder)
    {
        binder.Bind(singleton);
        binder.Bind(initOrder);
    }

    public void PostInject()
    {
        initOrder.Add(nameof(Test5SharedContext));
    }
}
