// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

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
/// Test4 scene is for testing InjectObjectPool.
/// </summary>
[TestFixture]
public class Scene4Tests_InjectObjectPool
{
    private bool initialized = false;

    private Test4LeafObject prefab;

    private Transform parentA;
    private Transform parentAInternal;

    private Transform parentB;
    private Transform parentBInternal;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();

            SceneManager.LoadScene("TestInject4", LoadSceneMode.Single);

            yield return null;

            prefab = Resources.Load<Test4LeafObject>("Test4Prefab");

            parentA = GameObject.Find("ParentA").transform;
            parentAInternal = GameObject.Find("ParentA/Internal").transform;

            parentB = GameObject.Find("ParentB").transform;
            parentBInternal = GameObject.Find("ParentB/Internal").transform;
        }
    }

    [Test]
    public void Test4Scene_Spawn()
    {
        var pool = InjectObjectPool.Create(prefab, false);

        List<Test4LeafObject> list = new List<Test4LeafObject>();

        for (int i = 0; i < 10; ++i)
        {
            var spawned = pool.Spawn(parentA);

            Assert.AreEqual(i + 2, parentA.childCount);
            Assert.AreEqual(100, spawned.Power);
            Assert.AreEqual(50, spawned.Health);
            Assert.AreEqual(parentAInternal, spawned.internalRef);
            Assert.AreEqual("Test Scene 4", spawned.bindedNew.Value);

            list.Add(spawned);
        }

        foreach (var spawned in list)
        {
            pool.Despawn(spawned);
        }

        Assert.AreEqual(1, parentA.childCount);
        Assert.AreEqual(10, pool.CountInactive);

        pool.Dispose();
    }

    [Test]
    public void Test4Scene_ReuseObject()
    {
        var pool = InjectObjectPool.Create(prefab, false);

        for (int i = 0; i < 10; ++i)
        {
            var spawned = pool.Spawn(parentA);

            Assert.AreEqual(100, spawned.Power);
            Assert.AreEqual(50, spawned.Health);
            Assert.AreEqual(parentAInternal, spawned.internalRef);
            Assert.AreEqual("Test Scene 4", spawned.bindedNew.Value);

            pool.Despawn(spawned);
        }

        for (int i = 0; i < 10; ++i)
        {
            var spawned = pool.Spawn(parentB);

            Assert.AreEqual(200, spawned.Power);
            Assert.AreEqual(70, spawned.Health);
            Assert.AreEqual(parentBInternal, spawned.internalRef);
            Assert.AreEqual("Test Scene 4", spawned.bindedNew.Value);
        }

        pool.Dispose();
    }
}