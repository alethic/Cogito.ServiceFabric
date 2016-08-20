using System.Fabric;
using System.Threading.Tasks;

using Microsoft.Owin;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    internal sealed class OwinStatelessService :
        Http.OwinStatelessService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public OwinStatelessService(StatelessServiceContext context)
            : base(context, "OwinStatelessService")
        {

        }

        protected override async Task OnRequest(IOwinContext context)
        {
            var actor = ActorProxy.Create<ITestActor>(new ActorId(123));
            await actor.IncrementThing();
            await actor.Connect();
        }

    }

}
