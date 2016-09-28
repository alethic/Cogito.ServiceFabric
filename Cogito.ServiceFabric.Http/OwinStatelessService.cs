using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Owin;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

using Owin;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Describes a service that exposes an OWIN endpoint.
    /// </summary>
    public abstract class OwinStatelessService :
        Cogito.ServiceFabric.StatelessService
    {

        readonly string appRoot;
        readonly string endpointName;
        readonly bool restartOnConfigurationPackageChange;
        OwinCommunicationListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appRoot"></param>
        /// <param name="endpointName"></param>
        /// <param name="restartOnConfigurationPackageChange"></param>
        public OwinStatelessService(
            StatelessServiceContext context,
            string appRoot,
            string endpointName = "HttpServiceEndpoint",
            bool restartOnConfigurationPackageChange = false)
            : base(context)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(appRoot != null);
            Contract.Requires<ArgumentNullException>(endpointName != null);

            this.appRoot = appRoot;
            this.endpointName = endpointName;
            this.restartOnConfigurationPackageChange = restartOnConfigurationPackageChange;
        }

        /// <summary>
        /// Creates the communication listener to expose this service over HTTP.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(ctx => listener = new OwinCommunicationListener(ConfigureInternal, ctx, endpointName, appRoot, restartOnConfigurationPackageChange));
        }

        /// <summary>
        /// Restarts the OWIN listener if it is currently started.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task RestartWebServer(CancellationToken cancellationToken)
        {
            if (listener != null)
            {
                await listener.RestartWebServer(cancellationToken);
            }
        }

        /// <summary>
        /// Configures the <see cref="IAppBuilder"/>.
        /// </summary>
        /// <param name="app"></param>
        void ConfigureInternal(IAppBuilder app)
        {
            Contract.Requires<ArgumentNullException>(app != null);

            // attach service to context
            app.Use(async (context, next) =>
            {
                context.SetService(this);

                if (next != null)
                    await next();
            });

            // begin user configuration
            Configure(app);
        }

        /// <summary>
        /// Configures the <see cref="IAppBuilder"/>.
        /// </summary>
        /// <param name="app"></param>
        protected virtual void Configure(IAppBuilder app)
        {
            Contract.Requires<ArgumentNullException>(app != null);

            app.Use(OnRequest);
        }

        /// <summary>
        /// Invoked when an incoming request is received unless the user has overridden the <see cref="Configure(IAppBuilder)"/> method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        protected virtual Task OnRequest(IOwinContext context, Func<Task> next)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            return next();
        }

    }

}
