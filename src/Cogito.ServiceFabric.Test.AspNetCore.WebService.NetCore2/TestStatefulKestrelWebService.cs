using System.Fabric;

using Autofac;

using Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac;
using Cogito.ServiceFabric.Services.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.NetCore2
{

    [RegisterStatefulService]
    public class TestStatefulKestrelWebService : StatefulKestrelWebService<Startup>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        public TestStatefulKestrelWebService(StatefulServiceContext context, ILifetimeScope scope) :
            base(context, scope)
        {

        }

    }

}
