using System;
using System.Fabric;

using Microsoft.Owin;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Provides extension methods for working with the <see cref="IOwinContext"/>.
    /// </summary>
    public static class OwinContextExtensions
    {

        /// <summary>
        /// Gets the OWIN environment key in which the <see cref="StatelessService"/> instance can be retrieved.
        /// </summary>
        public const string OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY = "fabric.Service";

        /// <summary>
        /// Gets the OWIN environment key in which the <see cref="ServiceContext"/> are stored.
        /// </summary>
        public const string OWIN_ENVIRONMENT_SERVICE_INIT_KEY = "fabric.ServiceContext";

        /// <summary>
        /// Gets the <see cref="StatelessService"/> instance from the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static StatelessService GetStatelessService(this IOwinContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return (StatelessService)context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY];
        }

        /// <summary>
        /// Sets the <see cref="StatelessService"/> instance on the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="service"></param>
        internal static void SetService(this IOwinContext context, StatelessService service)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY] = service;
            context.Environment[OWIN_ENVIRONMENT_SERVICE_INIT_KEY] = service.Context;
        }

        /// <summary>
        /// Gets the <see cref="StatefulService"/> instance from the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static StatefulService GetStatefulService(this IOwinContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return (StatefulService)context.Environment[OWIN_ENVIRONMENT_SERVICE_INSTANCE_KEY];
        }

        /// <summary>
        /// Sets the <see cref="StatefulService"/> instance on the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="service"></param>
        internal static void SetService(this IOwinContext context, StatefulService service)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (service == null)
                throw new ArgumentNullException(nameof(service));

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
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return (ServiceContext)context.Environment[OWIN_ENVIRONMENT_SERVICE_INIT_KEY];
        }

    }

}
