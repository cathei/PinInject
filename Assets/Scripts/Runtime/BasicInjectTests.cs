// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class BasicInjectTests : IContext
{
    [Inject]
    private IDependencyContainer _container;

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
        Pin.Inject(this);
    }

    public void Configure(DependencyRegistry registry)
    {
        registry.Add<IBindWithInterface>(new BindWithInterface(1));
        registry.Add(new BindWithNew());
    }

    [Test]
    public void BasicInject_Test()
    {
        var obj = new FlatInjectableClass();

        Pin.Inject(obj, _container);

        Assert.AreEqual(1, obj.bindedField.Value);
        Assert.AreEqual("Default Value", obj.BindedProperty.Value);
    }

    [Test]
    public void BasicInject_MustBeSame()
    {
        var obj1 = new FlatInjectableClass();
        var obj2 = new FlatInjectableClass();

        Pin.Inject(obj1, _container);
        Pin.Inject(obj2, _container);

        Assert.AreEqual(obj1.bindedField, obj2.bindedField);
        Assert.AreEqual(obj1.BindedProperty, obj2.BindedProperty);
    }
}