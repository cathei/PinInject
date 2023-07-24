// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test2SceneContext : MonoBehaviour, IInjectionContext
{
    public void Configure(DependencyBinder binder)
    {
        binder.Bind("Defense", 0);
        binder.Bind<IBindWithInterface>(new BindWithInterface(1024));
        binder.Bind(new BindWithNew("TestScene2"));
    }
}
