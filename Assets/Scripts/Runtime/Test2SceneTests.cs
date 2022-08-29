using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Test2 scene is for prefab variant and instantiation.
/// </summary>
[TestFixture]
public class Test2SceneTests
{
    private bool initialized = false;

    // test game object instantiation
    private GameObject originalPrefab;

    // test component instantiation
    private Test2SceneContext variantPrefab;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            Pin.Reset();
            SceneManager.LoadScene("TestInject2", LoadSceneMode.Single);

            originalPrefab = Resources.Load<GameObject>("Scene2Prefab");
            variantPrefab = Resources.Load<Test2SceneContext>("Scene2PrefabVariant");

            yield return null;
        }
    }

    [Test]
    public void Test2Scene_Instantiate()
    {
        var original = Pin.Instantiate(originalPrefab);
    }

    [Test]
    public void Test2Scene_MultiInstantiate()
    {
        var original1 = Pin.Instantiate(originalPrefab);
        var original2 = Pin.Instantiate(originalPrefab);
    }

    [Test]
    public void Test2Scene_VariantInstantiate()
    {
        var original = Pin.Instantiate(originalPrefab);
        var variant = Pin.Instantiate(variantPrefab);
    }
}