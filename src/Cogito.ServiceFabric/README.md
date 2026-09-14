# Cogito.ServiceFabric

Base types for Service Fabric applications: reading the fabric environment, and describing endpoints.

## Why

Code running under Service Fabric needs to know things about where it is — node name, application
name, the endpoints declared in the service manifest — and reaching for the environment variables and
`FabricRuntime` directly spreads that knowledge everywhere.

## Install

```shell
dotnet add package Cogito.ServiceFabric
```

## Use

`FabricEnvironment` exposes what the runtime publishes about the current node, application and code
package. `DefaultServiceEndpoint` describes an endpoint from the service manifest so callers can
resolve it without repeating its name.

This is the base the other `Cogito.ServiceFabric.*` packages build on.

## License

MIT.
