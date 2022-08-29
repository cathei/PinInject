using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test2GameObjectContext : MonoBehaviour, IInjectContext
{
    public int power;
    public int health;
    public Transform internalReference;

    public void Configure(InjectContainer container)
    {
        container.Bind("Power", power);
        container.Bind("Health", health);
        container.Bind(internalReference);
    }
}
