[![openupm](https://img.shields.io/npm/v/com.cathei.pininject?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.cathei.pininject/) [![GitHub](https://img.shields.io/github/license/cathei/PinInject)](https://github.com/cathei/PinInject/blob/main/LICENSE) [![Discord](https://img.shields.io/discord/942240862354702376?color=%235865F2&label=discord&logo=discord&logoColor=%23FFFFFF)](https://dsc.gg/coffcook)

# PinInject 📌
Minimalistic Dependency Injection tool for Unity

> **Warning**  
> PinInject is under development, current stage is: `Alpha`

## Dependency Injection
* https://en.wikipedia.org/wiki/Dependency_injection
* https://www.jamesshore.com/v2/blog/2006/dependency-injection-demystified
* https://www.sebaslab.com/ioc-container-unity-part-1
* https://moderncsharpinunity.github.io/post/dependency-injection-on-unity

## Why PinInject?
* Minimal API & complexity
* No unnecessary overconfiguration
* Anti-patterns are not allowed
* Static API gives you flexibility
* Utilities like Auto-Inject Collection and Object Pool
* Supports easy UI Binding with minimal Editor work

## Installation
You can install `PinInject` with Unity Package Manager, choose `Add package from git URL...`
```
https://github.com/cathei/PinInject.git
```
You can also install it via [OpenUPM](https://openupm.com/packages/com.cathei.pininject/)
```
openupm add com.cathei.pininject
```

## Where is Everything?
PinInject's goal is to be the most minimalistic DI Container available for Unity, with simplest possible API. Because I've learned that **People will not use DI unless it is simple**.

Also, Unity's GameObject and Component architecture already provides you `Service Locator Pattern`, which has similiar purpose to `Dependency Injection`. Which can cover edge-case of PinInject.

### PinInject **doesn't** support Lazy Binding
Strange, is it? Most DI container supports lazy binding by default. But PinInject doesn't. Your object will manage injection context's lifecycle. Your object's hierarchy will match with injection hierarchy. Since PinInject doesn't support lazy binding, you wouldn't have to worry about circular dependency.

### PinInject **doesn't** support Constructor Injection
You can manually pass dependencies by constructor whenever possible. PinInject is not made to call constructor instead of you. It's designed to support edge-case when it's hard to use manual dependency injection through constructor.

### PinInject **does** support static APIs
Problem with static APIs are that you need to pass context. But in Unity, GameObject already has own context: the hierarchy. Thus, your context should be inferred from your hierarchy. All you need is to do is adding `SceneInjectRoot` then replace `Instantiate` to `Pin.Instantiate`, it will magically instantiate your GameObject with dependency injection!

## Defining Context
In PinInject, you can define `Global`, `Scene` and `GameObject` context. Let's define Global context

```cs
public class MyGlobalContext : IInjectContext
{
    public void Configure(IInjectBinder binder)
    {
        binder.Bind(new GameManager());
        binder.Bind(new ItemManager());
        // ...
    }

    [GlobalInjectRoot]
    static void SetUp()
    {
        Pin.AddGlobalContext<MyGlobalContext>();
    }
}
```
`[GlobalInjectRoot]` is a wrapper for Unity's `[RuntimeInitializeOnLoadMethod]`. The Global context will be applied **any** GameObject or regular C# object that injected through PinInject.

Then add `SceneInjectRoot` to your scene (Right click on your Hierarchy and select `PinInject/Scene Inject Root`). This is component that triggers injection when scene loading.

Now you can add `[Inject]` to your field or property to inject in your component.

## Injecting into MonoBehaviour
```cs
public class MyComponent : MonoBehaviour
{
    // injection works with any field
    [Inject]
    private GameManager gameManager;

    // injection works with any properties with setter
    [Inject]
    public ItemManager ItemManager { get; private set; }
}
```
Now you don't have to reference singleton. It will work just like how Unity inspector injects value for you.

## Defining Scene Context and GameObject Context
Any `IInjectContext` Component under `SceneInjectRoot` becomes Scene Context and will affect every object in that scene and instantiated to the scene.
```cs
public class MySceneContext : MonoBehaviour, IInjectContext
{
    // assigned from inspector
    public GameObject sceneObject;

    public void Configure(IInjectBinder binder)
    {
        // shows named injection
        binder.Bind("MySceneObject", sceneObject);
    }
}
```

Otherwise, `IInjectContext` Component will be GameObject Context. GameObject context will be applied to any MonoBehaviour on same transform or it's children.

```cs
public class MyGameObjectContext : MonoBehaviour, IInjectContext
{
    // injected from global context
    [Inject]
    private GameManager gameManager;

    // injected from scene context (named injection)
    [Inject("MySceneObject")]
    private GameObject sceneObject;

    // assigned from inspector
    public string playerName;

    public void Configure(IInjectBinder binder)
    {
        Player player = gameManager.GetPlayer(playerName);

        // binding with interface
        // IPlayer will be injected to children game objects
        binder.Bind<IPlayer>(player);
    }
}
```

## Named Injection
You can perform named injection by `Bind("name", instance)` and using same name as `[Inject("name")]`.

## Injecting into C# object
Just like when you inject to GameObject, you can create hierarchical context for regular C# objects. By using `ResolveAttribute`, you can inject recursively to your children, applying context.
```cs
public class InjectedChild : IPostInjectHandler
{
    [Inject]
    private string injectedValue;

    public void PostInject()
    {
        Debug.Log(injectedValue);
    }
}

public class InjectedParent : IInjectContext
{
    private string value = "injection completed";

    [Inject]
    private GameManager gameManager;

    [Resolve]
    private InjectedChild child = new();

    public void Configure(IInjectBinder binder)
    {
        binder.Bind(value);
    }
}
```
Executed `Pin.Inject(new InjectedParent());` to show the value.

`InjectContainer` holds hierarchical information that injected into specific object.
For `GameObject`, you don't have to provide `InjectContainer`.

## About Caches
PinInject attaches small component to your prefab to cache components, so it will automatically converted into instance reference when you instantiate.  Also of course, it caches reflection result to improve performance.

## Using Collections
There is `InjectCollection` and `InjectKeyedCollection`. These collections will inject to collection's member automatially when item is added to collection.

> **Warning**  
> These collections should not contain `GameObject` or `Component`.

## Using Object Pools
PinInject includes `GenericObjectPool` for C# objects, and `InjectObjectPool` for GameObjects. `InjectObjectPool` automatically injects when you call `Spawn`.

## Using UI Binding
```cs
public class MyContext : MonoBehaviour, IInjectContext, IPostInjectHandler
{
    private EventSource<string> textEvent;
    private EventSource<object> buttonEvent;

    private int buttonClickCount = 0;

    public void Configure(IInjectBinder binder)
    {
        textEvent = new EventSource<string>();
        buttonEvent = new EventSource<object>();

        binder.BindEventSource("MyText", textEvent);
        binder.BindEventSource("MyButton", buttonEvent);
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
```
First add `MyContext` to parent object of `Text` and `Button`. Add `UITextBinder` on `Text` and set identifier to `MyText`. Then add `UIButtonOnClickDispatcher` on `Button` and set identifier to `MyButton`. Now you can see `Text` changes when `Button` clicked.

Have you noticed you didn't have to drag-drop anything from Unity Inspector? Using UI binding with PinInject, you can make your UI structure flexible and easily modifiable.
