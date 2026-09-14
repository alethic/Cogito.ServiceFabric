using System;
using System.Fabric;

using Microsoft.AspNetCore.Builder;

namespace Cogito.ServiceFabric.AspNetCore
{

    public static class ServiceFabricReverseProxyMiddlewareExtensions
    {

        /// <summary>
        /// Extension method to use <see cref="ServiceFabricReverseProxyMiddleware"/> for Service Fabric stateful or stateless
        /// services.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceFabricReverseProxyMiddleware(this IApplicationBuilder builder, ServiceContext context)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return builder.UseMiddleware<ServiceFabricReverseProxyMiddleware>(context);
        }

    }

}
