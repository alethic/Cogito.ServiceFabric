using System.Fabric;

using Autofac;

using Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac;
using Cogito.ServiceFabric.Services.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.Net47
{

    [RegisterStatelessService]
    public class TestStatelessKestrelWebService : StatelessKestrelWebService<Startup>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        public TestStatelessKestrelWebService(StatelessServiceContext context, ILifetimeScope scope) :
            base(context, scope)
        {

        }

    }

}
