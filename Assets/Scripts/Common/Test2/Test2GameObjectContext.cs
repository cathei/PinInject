// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test2GameObjectContext : MonoBehaviour, IInjectionContext
{
    public int power;
    public int health;
    public Transform internalReference;

    public void Configure(DependencyBinder binder)
    {
        binder.Bind("Power", power);
        binder.Bind("Health", health);
        binder.Bind(internalReference);
    }
}
