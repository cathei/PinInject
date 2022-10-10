// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test5SharedContext : MonoBehaviour, IContext, IPostInjectHandler
{
    public Test5Singleton singleton;

    public List<string> initOrder = new List<string>();

    public void Configure(DependencyRegistry registry)
    {
        registry.Add(singleton);
        registry.Add(initOrder);
    }

    public void PostInject()
    {
        initOrder.Add(nameof(Test5SharedContext));
    }
}
