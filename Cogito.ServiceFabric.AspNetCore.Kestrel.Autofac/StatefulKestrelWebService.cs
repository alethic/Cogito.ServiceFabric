using System;
using System.Fabric;

using Autofac;

using Cogito.AspNetCore.Autofac;

using Microsoft.AspNetCore.Hosting;

namespace Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac
{

    /// <summary>
    /// Defines a Service Fabric stateful web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class StatefulKestrelWebService<TStartup> :
        StatefulKestrelWebService,
        IStatefulWebService<TStartup>
        where TStartup : class
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="endpoint"></param>
        public StatefulKestrelWebService(
            StatefulServiceContext context,
            ILifetimeScope scope,
            WebServiceEndpoint endpoint = null) :
            base(context, scope, endpoint)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <summary>
        /// Configures the web host builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return base.ConfigureWebHostBuilder(builder).UseStartup<TStartup>(scope);
        }

    }

    /// <summary>
    /// Defines a Service Fabric stateful web service that hosts a Kestrel powered ASP.Net Core application.
    /// </summary>
    public abstract class StatefulKestrelWebService :
        Cogito.ServiceFabric.AspNetCore.Kestrel.StatefulKestrelWebService
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="endpoint"></param>
        public StatefulKestrelWebService(
            StatefulServiceContext context,
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
        /// Configures the web host builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            return base.ConfigureWebHostBuilder(builder).UseComponentContext(scope);
        }

    }

}
