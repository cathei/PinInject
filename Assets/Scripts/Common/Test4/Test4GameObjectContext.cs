// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test4GameObjectContext : MonoBehaviour, IContext
{
    public int power;
    public int health;
    public Transform internalReference;

    public void Configure(DependencyRegistry registry)
    {
        registry.Add("Power", power);
        registry.Add("Health", health);
        registry.Add(internalReference);
    }
}
