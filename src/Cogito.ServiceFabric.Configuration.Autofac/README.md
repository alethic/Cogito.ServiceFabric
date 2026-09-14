# Cogito.ServiceFabric.Configuration.Autofac

Contributes Service Fabric settings to the container's configuration.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Configuration.Autofac
```

## Use

```csharp
builder.RegisterAllAssemblyModules();
```

`ServiceFabricConfigurationBuilderConfiguration` adds the fabric configuration package as a source to
the `IConfiguration` built by `Cogito.Extensions.Configuration.Autofac`, so options types bound from
configuration pick up `Settings.xml` values with no extra wiring.

## License

MIT.
