// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test2SceneContext : MonoBehaviour, IContext
{
    public void Configure(DependencyRegistry registry)
    {
        registry.Add<IBindWithInterface>(new BindWithInterface(1024));
        registry.Add(new BindWithNew("TestScene2"));
    }
}
