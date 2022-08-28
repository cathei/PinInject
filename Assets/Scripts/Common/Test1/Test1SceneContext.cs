using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1SceneContext : MonoBehaviour, ISceneInjectContext
{
    public Test1SceneObject sceneObject;

    public void Configure(InjectContainer container)
    {
        container.Bind<IBindWithInterface>(new BindWithInterface(99));
        container.Bind(new BindWithNew("TestScene1"));
        container.Bind(sceneObject);
    }
}
