using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
public class Test1SceneTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        Pin.Initialize();
        SceneManager.LoadScene("TestInject1");
    }

    [Test]
    public void Test1Scene_RootLeaf()
    {
        var sceneObj = GameObject.Find("SceneObject");
        var rootLeaf = GameObject.Find("Root/Leaf").GetComponent<Test1LeafObject>();

        Assert.AreEqual(99, rootLeaf.BindWithInterface.Value);
        Assert.AreEqual("TestScene1", rootLeaf.BindWithNew.Value);
        Assert.AreSame(sceneObj, rootLeaf.SceneObject);
    }

    [Test]
    public void Test1Scene_ContextLeaf()
    {
        var contextLeaf1 = GameObject.Find("Root/Context").GetComponent<Test1LeafObject>();
        var contextLeaf2 = GameObject.Find("Root/Context/Leaf").GetComponent<Test1LeafObject>();

        // Assert.AreEqual(99, rootLeaf.BindWithInterface.Value);
        // Assert.AreEqual("TestScene1", rootLeaf.BindWithNew.Value);

        // Assert.AreEqual(, contextLeaf1.BindWithInterface.Value);
        // Assert.AreEqual("TestScene1", rootLeaf.BindWithNew.Value);
        // Assert.AreSame(sceneObj, rootLeaf.SceneObject);
    }

    [Test]
    public void Test1Scene_NestedContext()
    {
        var sceneObj = GameObject.Find("SceneObject");
        var rootLeaf = GameObject.Find("Root/Leaf").GetComponent<Test1LeafObject>();

        Assert.AreEqual(99, rootLeaf.BindWithInterface.Value);
        Assert.AreEqual("TestScene1", rootLeaf.BindWithNew.Value);
        Assert.AreSame(sceneObj, rootLeaf.SceneObject);
    }
}