// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using UnityEngine;

public class Test3SceneContext : MonoBehaviour, IInjectionContext
{
    public const string TestString = "TestString";

    public void Configure(DependencyBinder binder)
    {

    }
}
