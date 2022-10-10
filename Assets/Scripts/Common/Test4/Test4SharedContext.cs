// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test4SharedContext : MonoBehaviour, IInjectionContext
{
    public void Configure(DependencyBinder binder)
    {
        binder.Bind(new BindWithNew("Test Scene 4"));
    }
}
