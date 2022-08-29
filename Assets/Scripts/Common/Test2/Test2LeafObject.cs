using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public class Test2LeafObject : MonoBehaviour
{
    [Inject("Power")]
    public int Power { get; set; }

    [Inject("Health")]
    public int Health { get; set; }

    [Inject]
    public Transform internalRef;

    [Inject]
    public BindWithNew bindedNew;
}
