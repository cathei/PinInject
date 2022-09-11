using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cathei.PinInject;
using Cathei.PinInject.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ReflectionUnitTests
{
    public class InjectableClass
    {
        [Inject] public int value;
    }

    public class ResolvableClass
    {
        [Resolve] public InjectableClass injectable;
    }

    public class PostInjectClass : IPostInjectHandler
    {
        public void PostInject() { }
    }

    public class EmptyClass
    {
        public int value;
    }

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
    }

    [Test]
    public void ReflectionCache_Tests()
    {
        var injectable = ReflectionCache.Get(typeof(InjectableClass));
        var resolvable = ReflectionCache.Get(typeof(ResolvableClass));
        var postinject = ReflectionCache.Get(typeof(PostInjectClass));
        var empty = ReflectionCache.Get(typeof(EmptyClass));

        Assert.AreEqual(1, injectable.Injectables.Count());
        Assert.Null(injectable.Resolvables);
        Assert.True(injectable.HasAnyAttribute);

        Assert.Null(resolvable.Injectables);
        Assert.AreEqual(1, resolvable.Resolvables.Count());
        Assert.True(resolvable.HasAnyAttribute);

        Assert.Null(postinject.Injectables);
        Assert.Null(postinject.Resolvables);
        Assert.True(postinject.HasAnyAttribute);

        Assert.Null(empty.Injectables);
        Assert.Null(empty.Resolvables);
        Assert.False(empty.HasAnyAttribute);
    }

    [Test]
    public void ReflectionCache_Assertions()
    {
        var cacheComponent = ReflectionCache.Get(typeof(InjectCacheComponent));

        // cache component shouldn't be injected
        Assert.False(cacheComponent.HasAnyAttribute);

        var containerComponent = ReflectionCache.Get(typeof(InjectContainerComponent));

        // container component shouldn't be injected
        Assert.False(containerComponent.HasAnyAttribute);
    }
}