# Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac

Autofac wiring for Kestrel-hosted Service Fabric services.

## Install

```shell
dotnet add package Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac
```

## Use

```csharp
builder.RegisterAllAssemblyModules();
```

The Kestrel web service types resolve from the container, so the service and everything in the
request pipeline take their dependencies normally.

## License

MIT.
