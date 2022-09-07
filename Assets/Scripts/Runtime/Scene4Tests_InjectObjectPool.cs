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

    private GameObject prefab;
    private Transform parentA;
    private Transform parentB;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();
            SceneManager.LoadScene("TestInject4", LoadSceneMode.Single);

            prefab = Resources.Load<GameObject>("Test4Prefab");

            yield return null;
        }
    }

    [Test]
    public void Test4Scene_Spawn()
    {
        IInjectObjectPool pool = InjectObjectPool.Create(prefab, false);
    }

}