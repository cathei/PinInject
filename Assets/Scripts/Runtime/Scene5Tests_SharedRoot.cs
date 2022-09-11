using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

/// <summary>
/// Test4 scene is for testing SharedInjectRoot and singletons.
/// </summary>
[TestFixture]
public class Scene5Tests_SharedRoot
{
    private bool initialized = false;

    private Test5SharedContext _sharedContext;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();

            SceneManager.LoadScene("TestInject5", LoadSceneMode.Single);

            yield return null;

            _sharedContext = GameObject.FindObjectOfType<Test5SharedContext>();
        }
    }

    [Test, Order(1)]
    public void Test5Scene_SharedRoot()
    {
        var leaf = GameObject.Find("GameObjectLeaf").GetComponent<Test5GameObjectLeaf>();

        Assert.AreSame(_sharedContext.singleton, leaf.singleton);
    }

    [Test, Order(2)]
    public void Test5Scene_InitOrder()
    {
        string[] expected = new string[]
        {
            nameof(Test5SharedContext),
            nameof(Test5Singleton),
            nameof(Test5SceneContext),
            nameof(Test5SceneLeaf),
            nameof(Test5GameObjectLeaf)
        };

        Assert.AreEqual(expected.Length, _sharedContext.initOrder.Count);

        for (int i = 0; i < expected.Length; ++i)
        {
            Assert.AreEqual(expected[i], _sharedContext.initOrder[i]);
        }
    }

    [UnityTest]
    public IEnumerator Test5Scene_AdditiveLoad()
    {
        SceneManager.LoadScene("TestInject5", LoadSceneMode.Additive);

        yield return null;

        var leafs = GameObject.FindObjectsOfType<Test5GameObjectLeaf>();

        Assert.AreEqual(2, leafs.Length);

        foreach (var leaf in leafs)
        {
            Assert.AreSame(_sharedContext.singleton, leaf.singleton);
        }
    }

    [UnityTest]
    public IEnumerator Test5Scene_SingleLoad()
    {
        SceneManager.LoadScene("TestInject5", LoadSceneMode.Single);

        yield return null;

        var leaf = GameObject.Find("GameObjectLeaf").GetComponent<Test5GameObjectLeaf>();

        Assert.AreSame(_sharedContext.singleton, leaf.singleton);
    }
}