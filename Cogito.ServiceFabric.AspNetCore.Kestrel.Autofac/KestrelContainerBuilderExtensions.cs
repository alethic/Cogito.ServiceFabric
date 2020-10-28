using Autofac;

using Cogito.ServiceFabric.AspNetCore.Autofac;

namespace Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac
{

    public static class KestrelContainerBuilderExtensions
    {

        /// <summary>
        /// Registers the given <typeparamref name="TStartup"/> class to start in Kestrel stateful service.
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static void RegisterStatefulKestrelWebService<TService, TStartup>(this ContainerBuilder builder, string serviceTypeName, string endpointName = null)
            where TService : StatefulKestrelWebService<TStartup>
            where TStartup : class
        {
            builder.RegisterStatefulWebService<TService>(serviceTypeName, endpointName);
        }

        /// <summary>
        /// Registers the given <typeparamref name="TStartup"/> class to start in a Kestrel stateless service.
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static void RegisterStatelessKestrelWebService<TService, TStartup>(this ContainerBuilder builder, string serviceTypeName, string endpointName = null)
            where TService : StatelessKestrelWebService<TStartup>
            where TStartup : class
        {
            builder.RegisterStatelessWebService<TService>(serviceTypeName, endpointName);
        }

        /// <summary>
        /// Registers the given <typeparamref name="TStartup"/> class to start in Kestrel stateful service.
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static void RegisterStatefulKestrelWebService<TStartup>(this ContainerBuilder builder, string serviceTypeName, string endpointName = null)
            where TStartup : class
        {
            RegisterStatefulKestrelWebService<StatefulKestrelWebService<TStartup>, TStartup>(builder, serviceTypeName, endpointName);
        }

        /// <summary>
        /// Registers the given <typeparamref name="TStartup"/> class to start in a Kestrel stateless service.
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static void RegisterStatelessKestrelWebService<TStartup>(this ContainerBuilder builder, string serviceTypeName, string endpointName = null)
            where TStartup : class
        {
            RegisterStatelessKestrelWebService<StatelessKestrelWebService<TStartup>, TStartup>(builder, serviceTypeName, endpointName);
        }

    }

}
