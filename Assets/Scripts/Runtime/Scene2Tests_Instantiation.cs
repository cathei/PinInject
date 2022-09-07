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
public class Scene2Tests_Instantiation
{
    private bool initialized = false;

    // test game object instantiation
    private GameObject originalPrefab;

    // test component instantiation
    private Test2GameObjectContext variantPrefab;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();
            SceneManager.LoadScene("TestInject2", LoadSceneMode.Single);

            originalPrefab = Resources.Load<GameObject>("Test2Prefab");
            variantPrefab = Resources.Load<Test2GameObjectContext>("Test2PrefabVariant");

            yield return null;
        }
    }

    [Test]
    public void Test2Scene_Instantiate()
    {
        var original = Pin.Instantiate(originalPrefab);

        var leaf1 = original.transform.Find("Leaf1").GetComponent<Test2LeafObject>();
        var leaf2 = original.transform.Find("Leaf2").GetComponent<Test2LeafObject>();
        var nestedLeaf1 = original.transform.Find("Nested/Leaf1").GetComponent<Test2LeafObject>();

        Assert.AreSame(leaf1.internalRef, leaf2.internalRef);
        Assert.AreNotSame(leaf1.internalRef, nestedLeaf1.internalRef);
        Assert.AreSame(leaf1.transform, leaf2.internalRef);

        Assert.AreEqual(10, leaf1.Power);
        Assert.AreEqual(15, leaf1.Health);

        Assert.AreEqual(10, leaf2.Power);
        Assert.AreEqual(15, leaf2.Health);

        Assert.AreEqual(20, nestedLeaf1.Power);
        Assert.AreEqual(25, nestedLeaf1.Health);

    }

    [Test]
    public void Test2Scene_MultiInstantiate()
    {
        var original1 = Pin.Instantiate(originalPrefab);
        var original2 = Pin.Instantiate(originalPrefab);

        var leaf11 = original1.transform.Find("Leaf1").GetComponent<Test2LeafObject>();
        var leaf21 = original2.transform.Find("Leaf1").GetComponent<Test2LeafObject>();
        var leaf22 = original2.transform.Find("Leaf2").GetComponent<Test2LeafObject>();

        Assert.AreNotSame(leaf11.internalRef, leaf21.internalRef);
        Assert.AreSame(leaf21.internalRef, leaf22.internalRef);

        Assert.AreSame(leaf21.internalRef, leaf22.internalRef);
    }

    [Test]
    public void Test2Scene_VariantInstantiate()
    {
        var original = Pin.Instantiate(originalPrefab);
        var variant = Pin.Instantiate(variantPrefab);

        var leaf11 = original.transform.Find("Leaf1").GetComponent<Test2LeafObject>();
        var leaf21 = variant.transform.Find("Leaf1").GetComponent<Test2LeafObject>();
        var leaf22 = variant.transform.Find("Leaf2").GetComponent<Test2LeafObject>();

        Assert.AreEqual(10, leaf11.Power);
        Assert.AreEqual(15, leaf11.Health);

        Assert.AreEqual(30, leaf21.Power);
        Assert.AreEqual(35, leaf21.Health);

        var leaf23 = variant.transform.Find("Leaf3").GetComponent<Test2LeafObject>();
        var nestedLeaf22 = variant.transform.Find("Nested/Leaf2").GetComponent<Test2LeafObject>();

        Assert.AreEqual(30, leaf23.Power);
        Assert.AreEqual(35, leaf23.Health);

        Assert.AreEqual(40, nestedLeaf22.Power);
        Assert.AreEqual(25, nestedLeaf22.Health);

        Assert.AreSame(leaf23.transform, leaf21.internalRef);
        Assert.AreSame(leaf22.transform, nestedLeaf22.internalRef);
    }
}