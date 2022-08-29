using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class GlobalInjectTests
{
    public class GlboalInjectContext : IInjectContext
    {
        public void Configure(InjectContainer container)
        {
            container.Bind<IBindWithInterface>(new BindWithInterface(1));
            container.Bind(new BindWithNew());
        }
    }

    [SetUp]
    public void Setup()
    {
        Pin.Reset();
        Pin.AddGlobalContext<GlboalInjectContext>();
    }

    [Test]
    public void GlobalInject_Test()
    {
        var obj = new FlatInjectableClass();

        Pin.Inject(obj);

        Assert.AreEqual(1, obj.bindedField.Value);
        Assert.AreEqual("Default Value", obj.BindedProperty.Value);
    }

    [Test]
    public void GlobalInject_MustBeSame()
    {
        var obj1 = new FlatInjectableClass();
        var obj2 = new FlatInjectableClass();

        Pin.Inject(obj1);
        Pin.Inject(obj2);

        Assert.AreEqual(obj1.bindedField, obj2.bindedField);
        Assert.AreEqual(obj1.BindedProperty, obj2.BindedProperty);
    }
}