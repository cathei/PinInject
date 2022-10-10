// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.PinInject;
using Cathei.PinInject.UI;
using UnityEngine;

public class Test3UIContext : MonoBehaviour, IContext, IPostInjectHandler
{
    private EventSource<string> textEvent;
    private EventSource<object> buttonEvent;

    private int buttonClickCount = 0;

    public const string UI3Text = "UI3Text";
    public const string UI3Button = "UI3Button";

    public void Configure(DependencyRegistry registry)
    {
        textEvent = new EventSource<string>();
        buttonEvent = new EventSource<object>();

        registry.AddEventSource("UI3Text", textEvent);
        registry.AddEventSource("UI3Button", buttonEvent);
    }

    public void PostInject()
    {
        buttonEvent.Listeners += HandleButtonEvent;
    }

    private void HandleButtonEvent(object obj)
    {
        buttonClickCount++;
        textEvent.Publish("Button Clicked! " + buttonClickCount);
    }
}
