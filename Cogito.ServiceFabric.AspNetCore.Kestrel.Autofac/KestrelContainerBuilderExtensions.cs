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
        public static void RegisterStatefulKestrelWebService<TStartup>(this ContainerBuilder builder, string serviceTypeName, string endpointName = null)
            where TStartup : class
        {
            builder.RegisterStatefulWebService<StatefulKestrelWebService<TStartup>>(serviceTypeName, endpointName);
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
            builder.RegisterStatelessWebService<StatelessKestrelWebService<TStartup>>(serviceTypeName, endpointName);
        }

    }

}
