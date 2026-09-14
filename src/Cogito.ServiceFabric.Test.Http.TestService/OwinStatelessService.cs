using System;
using System.Fabric;
using System.Threading.Tasks;

using Cogito.ServiceFabric.Services.Autofac;

using Microsoft.Owin;

namespace Cogito.ServiceFabric.Test.Http.TestService
{

    [RegisterStatelessService]
    public class OwinStatelessService : Cogito.ServiceFabric.Http.OwinStatelessService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public OwinStatelessService(StatelessServiceContext context) :
            base(context, "", "OwinStatelessServiceEndpoint")
        {

        }

        protected override async Task OnRequest(IOwinContext context, Func<Task> next)
        {
            await context.Response.WriteAsync("Hello, World!");
            await base.OnRequest(context, next);
        }

    }

}
