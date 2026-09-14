# Cogito.ServiceFabric.Actors

A base actor with Cogito's state and reference helpers.

## Why

Reliable Actors put state behind `IActorStateManager`, whose API is get-try-set — so the common
"read it, change it, write it back" turns into several awaits and a conditional each time.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Actors
```

## Use

Derive from `Actor`, and work with state more directly:

```csharp
await StateManager.SetAsync("count", count + 1);
```

`ActorStateManagerExtensions` covers get-or-add and update-in-place; `GetActorReferences` enumerates
the actors an actor holds references to, which is how an actor addresses its peers without hard-coding
partition resolution.

## License

MIT.
