// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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
    [SetUp]
    public void Setup()
    {
        Pin.Reset();
    }

    [Test]
    public void Injection_EmptySceneRoot()
    {
        var go = new GameObject();
        go.AddComponent<SceneCompositionRoot>();

        Pin.Inject(go);

        var cache = go.GetComponent<HierarchyCacheComponent>();

        Assert.NotNull(cache);

        // one for parent and one for composition root
        Assert.AreEqual(2, cache.InnerReferences.Count);

        var container = go.GetComponent<DependencyContainerComponent>();

        Assert.NotNull(container);

        Object.Destroy(go);
    }
}