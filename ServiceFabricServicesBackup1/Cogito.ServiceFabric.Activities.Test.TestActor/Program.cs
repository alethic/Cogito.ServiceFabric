using System;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Activities.Test.TestActor
{

    static class Program
    {

        static void Main()
        {
            try
            {
                ActorRuntime.RegisterActorAsync<Test>().Wait();
                ActorRuntime.RegisterActorAsync<Test2>().Wait();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
