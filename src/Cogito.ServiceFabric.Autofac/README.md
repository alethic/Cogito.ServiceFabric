# Cogito.ServiceFabric.Autofac

Makes the Service Fabric environment available to an Autofac container.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Autofac
```

## Use

```csharp
builder.RegisterAllAssemblyModules();
```

The fabric environment and service context become resolvable, so components can depend on them
instead of reading `FabricRuntime` statically.

## License

MIT.
