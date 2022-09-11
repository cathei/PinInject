using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cathei.PinInject;
using Cathei.PinInject.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class InjectionUnitTests
{
    private UnityInjectStrategy _strategy = new UnityInjectStrategy();

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
    }

    [Test]
    public void Injection_EmptySceneRoot()
    {
        var go = new GameObject();
        go.AddComponent<SceneInjectRoot>();

        Pin.Inject(go);

        var cache = go.GetComponent<InjectCacheComponent>();

        Assert.NotNull(cache);
        Assert.AreEqual(1, cache.InnerReferences.Count);

        var container = go.GetComponent<InjectContainerComponent>();

        Assert.NotNull(container);

        Object.Destroy(go);
    }
}