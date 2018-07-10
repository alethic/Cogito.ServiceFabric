using System;
using System.Fabric;

using Autofac;

using Cogito.AspNetCore.Autofac;

using Microsoft.AspNetCore.Hosting;

namespace Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac
{

    /// <summary>
    /// Defines a Service Fabric stateless web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class StatelessKestrelWebService<TStartup> :
        StatelessKestrelWebService,
        IStatelessWebService<TStartup>
        where TStartup : class
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="endpoint"></param>
        public StatelessKestrelWebService(
            StatelessServiceContext context,
            ILifetimeScope scope,
            WebServiceEndpoint endpoint = null) :
            base(context, scope, endpoint)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <summary>
        /// Adds additional configuration to the web host.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return base.ConfigureWebHostBuilder(builder).UseLifetimeScopeStartup<TStartup>(scope);
        }

    }

    /// <summary>
    /// Defines a Service Fabric stateless web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    public abstract class StatelessKestrelWebService :
        Cogito.ServiceFabric.AspNetCore.Kestrel.StatelessKestrelWebService
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="endpoint"></param>
        public StatelessKestrelWebService(
            StatelessServiceContext context,
            ILifetimeScope scope,
            WebServiceEndpoint endpoint = null) :
            base(context, endpoint)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <summary>
        /// Gets the lifetime scope to be used to resolve components.
        /// </summary>
        protected ILifetimeScope LifetimeScope => scope;

        /// <summary>
        /// Adds additional configuration to the web host.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return base.ConfigureWebHostBuilder(builder).UseLifetimeScope(scope);
        }

    }

}
