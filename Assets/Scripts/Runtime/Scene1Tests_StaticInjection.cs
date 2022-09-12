// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Test1 scene is for testing root and hierarchical injection.
/// </summary>
[TestFixture]
public class Scene1Tests_StaticInjection
{
    private bool initialized = false;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();
            SceneManager.LoadScene("TestInject1", LoadSceneMode.Single);

            yield return null;
        }
    }

    [Test]
    public void Test1Scene_RootLeaf()
    {
        var sceneObj = GameObject.Find("SceneObject");
        var rootLeaf = GameObject.Find("Root/Leaf").GetComponent<Test1LeafObject>();

        Assert.AreEqual(99, rootLeaf.BindWithInterface.Value);
        Assert.AreEqual("TestScene1", rootLeaf.BindWithNew.Value);
        Assert.AreSame(sceneObj, rootLeaf.SceneObject.gameObject);
    }

    [Test]
    public void Test1Scene_ContextLeaf()
    {
        var rootLeaf = GameObject.Find("Root/Leaf").GetComponent<Test1LeafObject>();
        var contextLeaf1 = GameObject.Find("Root/Context").GetComponent<Test1LeafObject>();
        var contextLeaf2 = GameObject.Find("Root/Context/Leaf").GetComponent<Test1LeafObject>();

        Assert.AreEqual(10101, contextLeaf1.BindWithInterface.Value);
        Assert.AreEqual(10101, contextLeaf2.BindWithInterface.Value);

        Assert.AreSame(contextLeaf1.BindWithInterface, contextLeaf2.BindWithInterface);
        Assert.AreNotSame(contextLeaf1.BindWithInterface, rootLeaf.BindWithInterface);
    }

    [Test]
    public void Test1Scene_NestedContext()
    {
        var contextLeaf = GameObject.Find("Root/Context/InnerContext/Leaf").GetComponent<Test1LeafObject>();

        Assert.AreEqual(20202, contextLeaf.BindWithInterface.Value);
    }
}