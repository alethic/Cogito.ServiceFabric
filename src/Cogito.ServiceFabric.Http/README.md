# Cogito.ServiceFabric.Http

OWIN hosting and HTTP client support for Service Fabric services.

## Why

Calling another fabric service over HTTP means resolving its partition, retrying when the replica has
moved, and distinguishing a transient failure from a real one. The `ICommunicationClient` machinery
exists for that, but you have to implement it.

## Install

```shell
dotnet add package Cogito.ServiceFabric.Http
```

## Calling a service

`HttpCommunicationClientFactory` produces clients that resolve through the fabric and re-resolve when
an endpoint moves; `HttpExceptionHandler` classifies failures so the retry policy can tell a moved
replica from a genuine error.

## Hosting

`OwinCommunicationListener` runs an OWIN pipeline as a service's listener; `OwinStatelessService` and
`OwinStatefulService` are the base classes. `UseFabricHealth` reports pipeline health back to the
fabric, and `GetServiceContext` reaches the service context from inside an OWIN request.

## License

MIT.
