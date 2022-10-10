// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class CircularInjectTests
{
    public class CircularChildA
    {
        [Inject]
        public CircularChildB childB;
    }

    public class CircularChildB
    {
        [Inject]
        public CircularChildA childA;
    }

    public class CircularParent : IInjectionContext
    {
        [Resolve]
        public CircularChildA childA;

        [Resolve]
        public CircularChildB childB;

        public void Configure(DependencyBinder binder)
        {
            childA = new CircularChildA();
            childB = new CircularChildB();

            binder.Bind(childA);
            binder.Bind(childB);
        }
    }

    public class RecursiveInjectChild : IInjectionContext
    {
        [Resolve]
        public RecursiveInjectChild item;

        [Inject]
        public int value;

        public void Configure(DependencyBinder binder)
        {
            binder.Bind(value + 1);
        }
    }

    public class RecursiveInjectParent : IInjectionContext
    {
        [Resolve]
        public RecursiveInjectChild item;

        public int value;

        public void Configure(DependencyBinder binder)
        {
            binder.Bind(value);
        }
    }

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
    }

    [Test]
    public void CircularInject_Test()
    {
        var parent = new CircularParent();

        Pin.Inject(parent);

        Assert.NotNull(parent.childA);
        Assert.NotNull(parent.childB);

        Assert.AreSame(parent.childA, parent.childB.childA);
        Assert.AreSame(parent.childB, parent.childA.childB);
    }

    [Test]
    public void RecursiveInject_TestSuccess()
    {
        var parent = new RecursiveInjectParent();
        var child1 = new RecursiveInjectChild();
        var child2 = new RecursiveInjectChild();
        var child3 = new RecursiveInjectChild();

        parent.item = child1;
        child1.item = child2;
        child2.item = child3;

        parent.value = 10;

        Pin.Inject(parent);

        Assert.AreEqual(10, child1.value);
        Assert.AreEqual(11, child2.value);
        Assert.AreEqual(12, child3.value);
    }

    [Test]
    public void RecursiveInject_TestException()
    {
        var parent = new RecursiveInjectParent();
        var child1 = new RecursiveInjectChild();
        var child2 = new RecursiveInjectChild();

        parent.item = child1;
        child1.item = child2;
        child2.item = child2;

        parent.value = 20;

        Assert.Throws<InjectionException>(() => Pin.Inject(parent));
    }
}