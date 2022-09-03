# PinInject 📌
Minimalistic Dependency Injection tool for Unity

> **Warning**  
> PinInject is under development, current stage is: `Prototyping`

## Dependency Injection


## Why PinInject?
* Minimal API & complexity
* No unnecessary overconfiguration
* Anti-patterns are not allowed
* Static API gives you flexibility

## Where is Everything?
PinInject's goal is to be the most minimalistic DI Container available for Unity, with simplest possible API. Because I've learned that **People will not use DI unless it is simple**.

Also, Unity's GameObject and Component architecture already provides you `Service Locator Pattern`, which has similiar purpose to `Dependency Injection`. Which can cover edge-case of PinInject.

### PinInject **doesn't** support Lazy Binding
Strange, is it? Most DI container supports lazy binding by default. But PinInject doesn't. Your object will manage injection context's lifecycle. Your object's hierarchy will match with injection hierarchy. Since PinInject doesn't support lazy binding, you wouldn't have to worry about circular dependency.

### PinInject **doesn't** support Constructor Injection
You can manually pass dependencies by constructor whenever possible. PinInject is not made to call constructor instead of you. It's designed to support edge-case when it's hard to use manual dependency injection through constructor.

### PinInject **does** support static APIs
Problem with static APIs are that you need to pass context. But in Unity, GameObject already has own context: the hierarchy. Thus, your context should be inferred from your hierarchy. All you need is to do is replace `Instantiate` to `Pin.Instantiate` then it will instantiate your GameObject with dependency injection!

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

    [SetUpGlobalContext]
    static void SetUp()
    {
        Pin.AddGlobalContext<MyGlobalContext>();
    }
}
```
`SetUpGlobalContextAttribute` is a wrapper for Unity's `RuntimeInitializeOnLoadMethodAttribute`. The Global context will be applied **any** GameObject or regular C# object that injected through PinInject. Now you can add `InjectAttribute` to your field or property to inject in your component.

## Injecting into MonoBehaviour
```cs
public class MyComponent : MonoBehaviour
{
    [Inject]
    private GameManager gameManager;
}
```
Now you don't have to reference singleton. It will work just like how Unity inspector injects value for you.

## Defining Scene Context and GameObject Context
You can define SceneContext through `ISceneInjectContext`. Any scene context that is on the scene will be applied. You can define GameObject context through `IInjectContext`. GameObject context will be applied to any MonoBehaviour on same transform or it's children.

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
