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
PinInject's goal is to be the most minimalistic DI Container available for Unity, with simplest possible API. Because I learned that **People will not use DI unless it is simple**.

Also, Unity's GameObject and Component architecture already provides you `Service Locator Pattern`, which has similiar purpose to `Dependency Injection`.

## Hierarchical Dependency Injection
Just like when you inject to GameObject, you can create hierarchical context for regular C# objects.

`InjectContainer` holds hierarchical information that injected into specific object.
For `GameObject`, you don't have to provide `InjectContainer`.

## DI Container that Caches
PinInject attaches small component to your prefab to cache components.
Also it caches reflection result to improve performance.
