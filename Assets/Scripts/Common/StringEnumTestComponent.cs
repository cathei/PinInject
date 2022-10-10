// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using UnityEngine;

public enum TestEnum
{
    Cat = 0,
    Dog = 1,
    Fish = 2,
    Hamster = 3,
    Bird = 4,
}

[Flags]
public enum TestFlags
{
    Apple = 1,
    Banana = 2,
    Cherry = 4,
    Durian = 8,
    Eggplant = 16,
    Fig = 32,
    Grape = 64,
}

public class StringEnumTestComponent : MonoBehaviour
{
    public StringEnum<TestEnum> singularEnum;
    public StringEnum<TestFlags> flagsEnum;

    public void Start()
    {
        Debug.Log(singularEnum == TestEnum.Fish);
        Debug.Log(TestEnum.Fish == singularEnum);
        Debug.Log(flagsEnum == (TestFlags.Apple | TestFlags.Banana));

        singularEnum = TestEnum.Cat;
        flagsEnum = TestFlags.Apple | TestFlags.Grape;
    }
}
