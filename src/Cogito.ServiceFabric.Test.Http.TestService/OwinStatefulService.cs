using System;
using System.Fabric;
using System.Threading.Tasks;

using Cogito.ServiceFabric.Services.Autofac;

using Microsoft.Owin;

namespace Cogito.ServiceFabric.Test.Http.TestService
{

    [RegisterStatefulService]
    public class OwinStatefulService : Cogito.ServiceFabric.Http.OwinStatefulService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public OwinStatefulService(StatefulServiceContext context) :
            base(context, "", "OwinStatefulServiceEndpoint")
        {

        }

        protected override async Task OnRequest(IOwinContext context, Func<Task> next)
        {
            await context.Response.WriteAsync("Hello, World!");
            await base.OnRequest(context, next);
        }

    }

}
