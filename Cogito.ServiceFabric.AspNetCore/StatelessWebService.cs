using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;

using Cogito.AspNetCore;
using Cogito.ServiceFabric.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace Cogito.ServiceFabric.AspNetCore
{

    /// <summary>
    /// Defines a Service Fabric stateless web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    public abstract class StatelessWebService :
        StatelessService
    {

        readonly WebServiceEndpoint endpoint;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="endpoint"></param>
        public StatelessWebService(
            StatelessServiceContext context,
            WebServiceEndpoint endpoint = null) :
            base(context)
        {
            this.endpoint = endpoint;
        }

        /// <summary>
        /// Gets the default endpoint name upon which to create a service instance listener.
        /// </summary>
        protected WebServiceEndpoint Endpoint => endpoint;

        /// <summary>
        /// Creates the service listener.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(serviceContext =>
                CreateCommunicationListener(
                    serviceContext,
                    endpoint?.EndpointName,
                    (url, listener) => ConfigureWebHostBuilder(CreateWebHostBuilder(serviceContext, url, listener)).Build()));
        }

        /// <summary>
        /// Override to create the <see cref="AspNetCoreCommunicationListener"/> implementation.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="endpointName"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        protected abstract AspNetCoreCommunicationListener CreateCommunicationListener(
            StatelessServiceContext serviceContext,
            string endpointName,
            Func<string, AspNetCoreCommunicationListener, IWebHost> build);

        /// <summary>
        /// Creates the basic WebHostBuilder.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="url"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        protected virtual IWebHostBuilder CreateWebHostBuilder(ServiceContext serviceContext, string url, AspNetCoreCommunicationListener listener)
        {
            return new WebHostBuilder()
                .UseUrls(url)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseReverseProxyIntegration)
                .UseServiceFabricReverseProxyRewrite(serviceContext)
                .UseReverseProxyRewrite();
        }

        /// <summary>
        /// Adds additional configuration to the web host.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return builder;
        }

    }

}
