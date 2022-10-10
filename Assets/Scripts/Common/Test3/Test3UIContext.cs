// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;
using Cathei.PinInject.UI;
using UnityEngine;

public class Test3UIContext : MonoBehaviour, IInjectionContext, IPostInjectHandler
{
    private EventSource<string> textEvent;
    private EventSource<object> buttonEvent;

    private int buttonClickCount = 0;

    public const string UI3Text = "UI3Text";
    public const string UI3Button = "UI3Button";

    public void Configure(DependencyBinder binder)
    {
        textEvent = new EventSource<string>();
        buttonEvent = new EventSource<object>();

        binder.BindEventSource("UI3Text", textEvent);
        binder.BindEventSource("UI3Button", buttonEvent);
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
