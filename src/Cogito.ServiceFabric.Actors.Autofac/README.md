# Cogito.ServiceFabric.Actors.Autofac

Registers actors with the fabric runtime by attribute, and resolves them from Autofac.

## Why

The actor runtime constructs actors itself, so an actor cannot take constructor dependencies — and
each actor type otherwise needs its own registration call at startup.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Actors.Autofac
```

## Use

```csharp
[RegisterActor]
public class OrderActor : Actor, IOrderActor
{
    public OrderActor(ActorService service, ActorId id, IOrderRepository orders)
        : base(service, id) { ... }
}
```

With module scanning the actor type is registered with the runtime and each instance is resolved from
the container.

## License

MIT.
