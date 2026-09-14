# Cogito.ServiceFabric.AspNetCore

Run an ASP.NET Core application as a Service Fabric service.

## Why

Hosting a web stack inside a reliable service means implementing a communication listener, opening on
the endpoint the manifest declared, and shutting down cleanly when the replica moves. That is the
same code in every service.

## Install

```shell
dotnet add package Cogito.ServiceFabric.AspNetCore
```

## Use

Derive from `StatelessWebService` or `StatefulWebService` and supply the web host; the listener,
endpoint binding and lifecycle are handled.

`ServiceFabricReverseProxyMiddleware` lets a service proxy to another service by name, resolving the
partition through the fabric rather than a fixed address:

```csharp
app.UseServiceFabricReverseProxyMiddleware();
```

`UseServiceFabricReverseProxyRewrite` rewrites URLs in the proxied response so links point back
through the proxy.

## License

MIT.
