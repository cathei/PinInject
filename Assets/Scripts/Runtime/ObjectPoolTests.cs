// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ObjectPoolTests
{
    [SetUp]
    public void Setup()
    {
        Pin.Reset();
    }

    [Test]
    public void ObjectPoolTests_Generation()
    {
        var pool = GenericObjectPool.Create<List<int>>();

        var firstList = pool.Get();
        pool.Release(firstList);

        for (int i = 0; i < 100; i++)
        {
            List<int> list = pool.Get();
            list.Add(i);

            Assert.AreSame(firstList, list);
            Assert.AreEqual(i + 1, list.Count);

            pool.Release(list);
        }

        Assert.AreEqual(1, pool.CountInactive);
    }

    [Test]
    public void ObjectPoolTests_Reset()
    {
        var pool = GenericObjectPool.Create<List<int>>(x => x.Clear());

        var firstList = pool.Get();
        pool.Release(firstList);

        for (int i = 0; i < 100; i++)
        {
            List<int> list = pool.Get();
            list.Add(i);

            Assert.AreSame(firstList, list);
            Assert.AreEqual(1, list.Count);

            pool.Release(list);
        }

        Assert.AreEqual(1, pool.CountInactive);
    }

    [Test]
    public void ObjectPoolTests_InitialSize()
    {
        var pool = GenericObjectPool.Create<object>(5);

        Assert.AreEqual(5, pool.CountInactive);

        pool.Get();

        Assert.AreEqual(4, pool.CountInactive);
    }

    [Test]
    public void ObjectPoolTests_MaxSize()
    {
        var pool = GenericObjectPool.Create(() => "abcd", 0, 40);

        List<string> temp = new List<string>();

        for (int i = 0; i < 100; i++)
        {
            string str = pool.Get();
            temp.Add(str);
        }

        foreach (var list in temp)
            pool.Release(list);

        Assert.AreEqual(40, pool.CountInactive);
    }

}
