using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test2SceneContext : MonoBehaviour, ISceneInjectContext
{
    public void Configure(IInjectBinder binder)
    {
        binder.Bind<IBindWithInterface>(new BindWithInterface(1024));
        binder.Bind(new BindWithNew("TestScene2"));
    }
}
