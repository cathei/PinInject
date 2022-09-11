using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test4SharedContext : MonoBehaviour, IInjectContext
{
    public void Configure(IInjectBinder binder)
    {
        binder.Bind(new BindWithNew("Test Scene 4"));
    }
}
