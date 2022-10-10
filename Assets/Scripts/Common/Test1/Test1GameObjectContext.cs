// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1GameObjectContext : MonoBehaviour, IContext
{
    public int value;

    public void Configure(DependencyRegistry registry)
    {
        registry.Add<IBindWithInterface>(new BindWithInterface(value));
    }
}
