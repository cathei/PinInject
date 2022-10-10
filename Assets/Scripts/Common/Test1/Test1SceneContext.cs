// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1SceneContext : MonoBehaviour, IContext
{
    public Test1SceneObject sceneObject;

    public void Configure(DependencyRegistry registry)
    {
        registry.Add<IBindWithInterface>(new BindWithInterface(99));
        registry.Add(new BindWithNew("TestScene1"));
        registry.Add(sceneObject);
    }
}
