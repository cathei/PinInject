using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class BasicInjectTests : IInjectContext
{
    [Inject]
    private IInjectContainer _container;

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
        Pin.Inject(this);
    }

    public void Configure(IInjectBinder binder)
    {
        binder.Bind<IBindWithInterface>(new BindWithInterface(1));
        binder.Bind(new BindWithNew());
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