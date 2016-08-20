using System;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    static class Program
    {

        static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("OwinStatelessServiceType", ctx => new OwinStatelessService(ctx)).Wait();
                ServiceRuntime.RegisterServiceAsync("OwinStatefulServiceType", ctx => new OwinStatefulService(ctx)).Wait();
                ActorRuntime.RegisterActorAsync<TestActor>((ctx, Type) => new ActorService(ctx, Type, settings: new ActorServiceSettings() { ActorGarbageCollectionSettings = new ActorGarbageCollectionSettings(60, 60) })).Wait();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }

}
