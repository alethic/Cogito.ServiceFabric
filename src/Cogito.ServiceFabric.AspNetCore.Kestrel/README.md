# Cogito.ServiceFabric.AspNetCore.Kestrel

Kestrel-hosted ASP.NET Core services for Service Fabric.

## Why

Kestrel needs to listen on the address the service manifest declared, and — for a stateful service —
on an address unique to the replica, or two replicas on one node collide.

## Install

```shell
dotnet add package Cogito.ServiceFabric.AspNetCore.Kestrel
```

## Use

Derive from `StatelessKestrelWebService` or `StatefulKestrelWebService`. The listener binds Kestrel to
the manifest endpoint, and the stateful form includes the partition and replica in the published
address so the fabric can route to the right replica.

## License

MIT.
