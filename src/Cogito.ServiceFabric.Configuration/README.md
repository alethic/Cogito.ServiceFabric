# Cogito.ServiceFabric.Configuration

Reads Service Fabric configuration packages through `IConfiguration`.

## Why

Fabric settings live in `Settings.xml` and are reached through `ConfigurationPackage` and its section
and parameter collections — an API unlike the one the rest of the application uses, and one that
changes underneath you when the package is upgraded.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Configuration
```

## Use

Add the fabric configuration source to a builder and read settings as ordinary configuration:

```csharp
var connection = configuration["Database:ConnectionString"];
```

Sections and parameters from the configuration package appear as configuration keys, so binding to an
options class works the same as with any other source.

## License

MIT.
