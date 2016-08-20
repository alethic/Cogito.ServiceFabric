using System;
using System.Threading.Tasks;

using Cogito.ServiceFabric.Http;

using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    [StatePersistence(StatePersistence.Persisted)]
    class TestActor :
        Actor<TestActorState>,
        ITestActor
    {

        protected override Task<TestActorState> CreateDefaultState()
        {
            return Task.FromResult(new TestActorState());
        }

        public Task IncrementThing()
        {
            State.Thing++;
            return Task.FromResult(true);
        }

        public async Task Connect()
        {
            var client = new ServicePartitionClient<HttpCommunicationClient>(
                HttpCommunicationClientFactory.Default,
                new Uri(new Uri(ApplicationName + "/"), "OwinStatefulService"));
            var o = await client.InvokeWithRetry(async c =>
            {
                return await c.Http.GetAsync(c.BaseAddress);
            }
            );
            o.EnsureSuccessStatusCode();
        }

    }

}
