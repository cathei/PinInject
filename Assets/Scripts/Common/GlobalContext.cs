using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class GlobalContext : IInjectContext
{
    public void Configure(IInjectBinder binder)
    {
        Debug.Log("Configuring Global Context");
    }

    [SetUpGlobalContext]
    public static void SetUp()
    {
        Pin.AddGlobalContext<GlobalContext>();
    }
}
