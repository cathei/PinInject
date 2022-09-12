// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1SceneContext : MonoBehaviour, IInjectContext
{
    public Test1SceneObject sceneObject;

    public void Configure(IInjectBinder binder)
    {
        binder.Bind<IBindWithInterface>(new BindWithInterface(99));
        binder.Bind(new BindWithNew("TestScene1"));
        binder.Bind(sceneObject);
    }
}
