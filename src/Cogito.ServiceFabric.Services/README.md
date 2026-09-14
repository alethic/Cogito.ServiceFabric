# Cogito.ServiceFabric.Services

Base classes and proxy helpers for Service Fabric reliable services.

## Why

Resolving a partition and building a proxy is boilerplate repeated at every call site, and the
default remoting serializer is binary — which makes a message on the wire impossible to read when
something goes wrong.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Services
```

## Use

Derive from `StatelessService` or `StatefulService` for a service that carries the Cogito plumbing,
and address another service through a reference rather than a resolved partition:

```csharp
var proxy = serviceReference.CreateProxy<IOrderService>();
```

`IServiceReference` / `ServiceReference` name a service and partition together, so the resolution
rules live in one place. `ServiceContextScope` flows the current service context to code that needs
it without threading it through every call.

`ServiceRemotingJsonSerializationProvider` swaps remoting's binary serialization for JSON — slower on
the wire, but readable in a trace and tolerant of a contract that has gained a property.

## License

MIT.
