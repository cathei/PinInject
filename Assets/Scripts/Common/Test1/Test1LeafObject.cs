using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test1LeafObject : MonoBehaviour
{
    [Inject]
    public IBindWithInterface BindWithInterface { get; set; }

    [Inject]
    public BindWithNew BindWithNew { get; set; }

    [Inject]
    public Test1SceneObject SceneObject { get; set; }
}
