// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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
