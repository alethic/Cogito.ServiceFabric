using System;
using System.Fabric;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Cogito.ServiceFabric.AspNetCore
{

    /// <summary>
    /// Adds in middleware that detects the Service Fabric Reverse Proxy and fixes up requests for ASP.Net.
    /// </summary>
    class ServiceFabricReverseProxyRewriteSetupFilter :
        IStartupFilter
    {

        readonly ServiceContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ServiceFabricReverseProxyRewriteSetupFilter(ServiceContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseServiceFabricReverseProxyMiddleware(context);
                next(app);
            };
        }

    }

}
