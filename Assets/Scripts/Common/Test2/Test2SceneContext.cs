using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test2SceneContext : MonoBehaviour, ISceneInjectContext
{
    public void Configure(InjectContainer container)
    {
        container.Bind<IBindWithInterface>(new BindWithInterface(1024));
        container.Bind(new BindWithNew("TestScene2"));
    }
}
