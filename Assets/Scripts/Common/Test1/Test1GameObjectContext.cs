using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1GameObjectContext : MonoBehaviour, IInjectContext
{
    public int value;

    public void Configure(InjectContainer container)
    {
        container.Bind<IBindWithInterface>(new BindWithInterface(value));
    }
}
