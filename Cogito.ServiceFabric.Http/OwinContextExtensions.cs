using System;
using System.Diagnostics.Contracts;
using System.Fabric;

using Microsoft.Owin;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Provides extension methods for working with the <see cref="IOwinContext"/>.
    /// </summary>
    public static class OwinContextExtensions
    {

        /// <summary>
        /// Gets the OWIN environment key in which the <see cref="IService"/> instance can be retrieved.
        /// </summary>
        public const string OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY = "cogito.Service";

        /// <summary>
        /// Gets the OWIN environment key in which the <see cref="ServiceContext"/> are stored.
        /// </summary>
        public const string OWIN_ENVIRONMENT_SERVICE_INIT_KEY = "cogito.ServiceContext";

        /// <summary>
        /// Gets the <see cref="IService"/> instance from the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static StatelessService GetStatelessService(this IOwinContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            return (StatelessService)context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY];
        }

        /// <summary>
        /// Sets the <see cref="IService"/> instance on the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="service"></param>
        internal static void SetService(this IOwinContext context, StatelessService service)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(service != null);

            context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY] = service;
            context.Environment[OWIN_ENVIRONMENT_SERVICE_INIT_KEY] = service.Context;
        }

        /// <summary>
        /// Gets the <see cref="IService"/> instance from the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static StatefulService GetStatefulService(this IOwinContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            return (StatefulService)context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY];
        }

        /// <summary>
        /// Sets the <see cref="IService"/> instance on the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="service"></param>
        internal static void SetService(this IOwinContext context, StatefulService service)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(service != null);

            context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY] = service;
            context.Environment[OWIN_ENVIRONMENT_SERVICE_INIT_KEY] = service.Context;
        }

        /// <summary>
        /// Gets the <see cref="ServiceInitializationParameters"/> instance from the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ServiceContext GetServiceContext(this IOwinContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            return (ServiceContext)context.Environment[OWIN_ENVIRONMENT_SERVICE_INIT_KEY];
        }

    }

}
