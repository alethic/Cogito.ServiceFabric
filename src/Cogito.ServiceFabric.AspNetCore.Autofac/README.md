# Cogito.ServiceFabric.AspNetCore.Autofac

Autofac wiring for ASP.NET Core services running on Service Fabric.

## Install

```shell
dotnet add package Cogito.ServiceFabric.AspNetCore.Autofac
```

## Use

```csharp
builder.RegisterAllAssemblyModules();
```

The web service types resolve from the container, with the service context available to components,
so a controller can depend on fabric services the same way it depends on anything else.

## License

MIT.
