// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test1GameObjectContext : MonoBehaviour, IInjectionContext
{
    public int value;

    public void Configure(DependencyBinder binder)
    {
        binder.Bind<IBindWithInterface>(new BindWithInterface(value));
    }
}
