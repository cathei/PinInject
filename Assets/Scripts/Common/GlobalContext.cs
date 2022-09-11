using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class SharedContext : IInjectContext
{
    public void Configure(IInjectBinder binder)
    {
        Debug.Log("Configuring Shared Context");
    }
}
