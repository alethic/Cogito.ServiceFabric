# Cogito.ServiceFabric

[![Build](https://github.com/alethic/Cogito.ServiceFabric/actions/workflows/Cogito.ServiceFabric.yml/badge.svg)](https://github.com/alethic/Cogito.ServiceFabric/actions/workflows/Cogito.ServiceFabric.yml)

Service Fabric services and actors resolved from Autofac, with ASP.NET Core hosting, configuration and HTTP support.

## Packages

**[Cogito.ServiceFabric](https://www.nuget.org/packages/Cogito.ServiceFabric)** — Base types for Service Fabric applications: reading the fabric environment, and describing endpoints.

**[Cogito.ServiceFabric.Actors](https://www.nuget.org/packages/Cogito.ServiceFabric.Actors)** — A base actor with Cogito's state and reference helpers.

**[Cogito.ServiceFabric.Actors.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.Actors.Autofac)** — Registers actors with the fabric runtime by attribute, and resolves them from Autofac.

**[Cogito.ServiceFabric.AspNetCore](https://www.nuget.org/packages/Cogito.ServiceFabric.AspNetCore)** — Run an ASP.NET Core application as a Service Fabric service.

**[Cogito.ServiceFabric.AspNetCore.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.AspNetCore.Autofac)** — Autofac wiring for ASP.NET Core services running on Service Fabric.

**[Cogito.ServiceFabric.AspNetCore.Kestrel](https://www.nuget.org/packages/Cogito.ServiceFabric.AspNetCore.Kestrel)** — Kestrel-hosted ASP.NET Core services for Service Fabric.

**[Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac)** — Autofac wiring for Kestrel-hosted Service Fabric services.

**[Cogito.ServiceFabric.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.Autofac)** — Makes the Service Fabric environment available to an Autofac container.

**[Cogito.ServiceFabric.Configuration](https://www.nuget.org/packages/Cogito.ServiceFabric.Configuration)** — Reads Service Fabric configuration packages through `IConfiguration`.

**[Cogito.ServiceFabric.Configuration.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.Configuration.Autofac)** — Contributes Service Fabric settings to the container's configuration.

**[Cogito.ServiceFabric.Http](https://www.nuget.org/packages/Cogito.ServiceFabric.Http)** — OWIN hosting and HTTP client support for Service Fabric services.

**[Cogito.ServiceFabric.Services](https://www.nuget.org/packages/Cogito.ServiceFabric.Services)** — Base classes and proxy helpers for Service Fabric reliable services.

**[Cogito.ServiceFabric.Services.Autofac](https://www.nuget.org/packages/Cogito.ServiceFabric.Services.Autofac)** — Registers Service Fabric services with the runtime by attribute, and resolves them from Autofac.

Each package carries its own README with the detail; the links above go to nuget.org.

## Building

```shell
dotnet restore Cogito.ServiceFabric.slnx
dotnet msbuild -p:Configuration=Release Cogito.ServiceFabric.dist.msbuildproj
```

Packages are staged into `dist/nuget` and test suites into `dist/tests`; run a suite with
`dotnet test -f <tfm> <path to its assembly>`.

## License

MIT — see [LICENSE](LICENSE).
