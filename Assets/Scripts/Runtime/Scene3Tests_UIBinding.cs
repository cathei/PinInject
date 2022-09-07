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
/// Test3 scene is for testing UI binding.
/// </summary>
[TestFixture]
public class Scene3Tests_UIBinding
{
    private bool initialized = false;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        if (!initialized)
        {
            initialized = true;

            Pin.Reset();
            SceneManager.LoadScene("TestInject3", LoadSceneMode.Single);

            yield return null;
        }
    }

    [Test]
    public void Test3Scene_ButtonClick()
    {
        var button = GameObject.Find("Canvas/Button").GetComponent<Button>();
        var text = GameObject.Find("Canvas/Button/Text").GetComponent<TMP_Text>();

        button.onClick.Invoke();
        button.onClick.Invoke();
        button.onClick.Invoke();

        Assert.AreEqual("Button Clicked! 3", text.text);
    }

    [Test]
    public void Test3Scene_DynamicInstantiation()
    {
        var prefab = GameObject.Find("Canvas/Button").GetComponent<Button>();

        var button1 = Pin.Instantiate(prefab, prefab.transform.parent);
        var text1 = button1.transform.Find("Text").GetComponent<TMP_Text>();

        var button2 = Pin.Instantiate(prefab, prefab.transform.parent);
        var text2 = button2.transform.Find("Text").GetComponent<TMP_Text>();

        button1.onClick.Invoke();
        button1.onClick.Invoke();

        button2.onClick.Invoke();

        Assert.AreEqual("Button Clicked! 2", text1.text);
        Assert.AreEqual("Button Clicked! 1", text2.text);
    }
}