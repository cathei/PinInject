// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test2LeafObject : MonoBehaviour
{
    [Inject("Power")]
    public int Power { get; set; }

    [Inject("Health")]
    public int Health { get; set; }

    [Inject("Defense")]
    public int Defense { get; set; }

    [Inject]
    public Transform internalRef;

    [Inject]
    public BindWithNew bindedNew;
}
