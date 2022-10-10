// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class PersistentContext : IContext
{
    public void Configure(DependencyRegistry registry)
    {
        Debug.Log("Configuring Shared Context");
    }
}
