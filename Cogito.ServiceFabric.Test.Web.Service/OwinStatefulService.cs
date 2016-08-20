using System;
using System.Fabric;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    internal sealed class OwinStatefulService :
        Http.OwinStatefulService,
        IOwinStatefulService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public OwinStatefulService(StatefulServiceContext context)
            : base(context, "OwinStatefulService")
        {

        }

        public Task<int> Ping()
        {
            return Task.FromResult(0);
        }

        protected override Task OnRequest(IOwinContext context)
        {
            return base.OnRequest(context);
        }

    }

}
