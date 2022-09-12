// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test4GameObjectContext : MonoBehaviour, IInjectContext
{
    public int power;
    public int health;
    public Transform internalReference;

    public void Configure(IInjectBinder binder)
    {
        binder.Bind("Power", power);
        binder.Bind("Health", health);
        binder.Bind(internalReference);
    }
}
