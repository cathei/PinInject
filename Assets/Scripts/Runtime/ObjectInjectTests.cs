using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ObjectInjectTests
{
    public class GlboalInjectContext : IInjectContext
    {
        public void Configure(InjectContainer container)
        {
            container.Bind<IBindWithInterface>(new BindWithInterface(8));
            container.Bind(new BindWithNew());
        }
    }

    public class InjectableChild
    {
        [Inject]
        public IBindWithInterface bindedInterf;

        [Inject]
        public string bindedStr;
    }

    public class InjectableParent : IInjectContext
    {
        [Inject]
        public IBindWithInterface bindedInterf;

        [Resolve]
        public InjectableChild child = new InjectableChild();

        [Resolve]
        public InjectableParent Nested { get; }

        private string _value;

        public InjectableParent(string value, InjectableParent nested)
        {
            _value = value;
            Nested = nested;
        }

        public void Configure(InjectContainer container)
        {
            container.Bind(_value);
        }
    }

    [SetUp]
    public void Setup()
    {
        Pin.Initialize();
        Pin.AddGlobalContext<GlboalInjectContext>();
    }

    [Test]
    public void ObjectInject_Test()
    {
        var parent = new InjectableParent("inject test", null);

        Pin.Inject(parent);

        Assert.AreEqual(null, parent.Nested);
        Assert.AreEqual("inject test", parent.child.bindedStr);
        Assert.AreSame(parent.bindedInterf, parent.child.bindedInterf);
    }

    [Test]
    public void ObjectInject_NestedTest()
    {
        var depth3 = new InjectableParent("depth3", null);
        var depth2 = new InjectableParent("depth2", depth3);
        var depth1 = new InjectableParent("depth1", depth2);

        Pin.Inject(depth1);

        Assert.AreEqual("depth1", depth1.child.bindedStr);
        Assert.AreEqual("depth2", depth2.child.bindedStr);
        Assert.AreEqual("depth3", depth3.child.bindedStr);
    }
}