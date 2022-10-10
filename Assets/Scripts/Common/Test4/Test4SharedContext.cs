// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test4SharedContext : MonoBehaviour, IContext
{
    public void Configure(DependencyRegistry registry)
    {
        registry.Add(new BindWithNew("Test Scene 4"));
    }
}
