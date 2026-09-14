# Cogito.ServiceFabric.Services.Autofac

Registers Service Fabric services with the runtime by attribute, and resolves them from Autofac.

## Why

Each service otherwise needs a registration call in `Program.Main` naming its service type string —
a second place to edit for every new service, and a string that has to match the manifest.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Services.Autofac
```

## Use

```csharp
[RegisterStatelessService("OrderServiceType")]
public class OrderService : StatelessService
{
    public OrderService(StatelessServiceContext context, IOrderRepository orders)
        : base(context) { ... }
}
```

With module scanning the service is registered with the fabric runtime and constructed from the
container, so it takes constructor dependencies like anything else.
`[RegisterStatefulService]` is the stateful equivalent.

## License

MIT.
