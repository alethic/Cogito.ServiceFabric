using System;
using System.Fabric;

using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;

namespace Cogito.ServiceFabric.AspNetCore.Kestrel
{

    /// <summary>
    /// Defines a Service Fabric stateless web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    public class StatelessKestrelWebService :
        StatelessWebService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="endpoint"></param>
        public StatelessKestrelWebService(
            StatelessServiceContext context,
            DefaultServiceEndpoint endpoint = null) :
            base(context, endpoint)
        {

        }

        /// <summary>
        /// Creates the Kestral communication listener.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="endpointName"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        protected override AspNetCoreCommunicationListener CreateCommunicationListener(
            StatelessServiceContext serviceContext,
            string endpointName,
            Func<string, AspNetCoreCommunicationListener, IWebHost> build)
        {
            if (endpointName != null)
                return new KestrelCommunicationListener(serviceContext, endpointName, build);
            else
                return new KestrelCommunicationListener(serviceContext, build);
        }

        /// <summary>
        /// Configures the application to use Kestral.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return base.ConfigureWebHostBuilder(builder).UseKestrel();
        }

    }

}
