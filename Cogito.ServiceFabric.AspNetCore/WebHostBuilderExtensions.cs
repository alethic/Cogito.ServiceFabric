using System;
using System.Fabric;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Cogito.ServiceFabric.AspNetCore
{

    public static class WebHostBuilderExtensions
    {

        /// <summary>
        /// Adds in middleware that detects the Service Fabric Reverse Proxy and fixes up requests for ASP.Net.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseServiceFabricReverseProxyRewrite(this IWebHostBuilder hostBuilder, ServiceContext context)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // check if UseServiceFabricReverseProxyRewrite was called already
            if (hostBuilder.GetSetting(nameof(UseServiceFabricReverseProxyRewrite)) != null)
                return hostBuilder;

            hostBuilder.UseSetting(nameof(UseServiceFabricReverseProxyRewrite), true.ToString());
            hostBuilder.ConfigureServices(services => services.AddSingleton<IStartupFilter>(new ServiceFabricReverseProxyRewriteSetupFilter(context)));

            return hostBuilder;
        }

    }

}
