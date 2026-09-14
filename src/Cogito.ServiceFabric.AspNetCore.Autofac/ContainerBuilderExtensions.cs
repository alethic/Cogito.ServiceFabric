using Autofac;
using Autofac.Builder;
using Autofac.Integration.ServiceFabric;

namespace Cogito.ServiceFabric.AspNetCore.Autofac
{

    public static class ContainerBuilderExtensions
    {

        /// <summary>
        /// Registers a stateless web service with the container.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterStatelessWebService<TService>(
            this ContainerBuilder builder,
            string serviceTypeName,
            string endpointName = null)
            where TService : StatelessWebService
        {
            return builder.RegisterStatelessService<TService>(serviceTypeName)
                .WithParameter(TypedParameter.From(new DefaultServiceEndpoint(endpointName)));
        }

        /// <summary>
        /// Registers a stateless web service with the container.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="endpointName"></param>
        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterStatefulWebService<TService>(
            this ContainerBuilder builder,
            string serviceTypeName,
            string endpointName = null)
            where TService : StatefulWebService
        {
            return builder.RegisterStatefulService<TService>(serviceTypeName)
                .WithParameter(TypedParameter.From(new DefaultServiceEndpoint(endpointName)));
        }

    }

}
