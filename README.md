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

## Hierarchical Dependency Injection
Just like when you inject to GameObject, you can create hierarchical context for regular C# objects.

`InjectContainer` holds hierarchical information that injected into specific object.
For `GameObject`, you don't have to provide `InjectContainer`.

## DI Container that Caches
PinInject attaches small component to your prefab to cache components.
Also it caches reflection result to improve performance.
