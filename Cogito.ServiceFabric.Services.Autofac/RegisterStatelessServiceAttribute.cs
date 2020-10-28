using System;

using Autofac;
using Autofac.Builder;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Services.Autofac
{

    /// <summary>
    /// Registers a stateless service class.
    /// </summary>
    public class RegisterStatelessServiceAttribute : Attribute, IRegistrationRootAttribute, IRegistrationBuilderAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RegisterStatelessServiceAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serviceTypeName"></param>
        public RegisterStatelessServiceAttribute(string serviceTypeName)
        {
            ServiceTypeName = serviceTypeName;
        }

        /// <summary>
        /// Specifies the name of the service type.
        /// </summary>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// Specifies the tag to apply to the lifetime scope that hosts the actor.
        /// </summary>
        public object LifetimeScopeTag { get; set; }

        /// <summary>
        /// Default endpoint to make available to the service instance as a <see cref="DefaultServiceEndpoint"/>.
        /// </summary>
        public string DefaultEndpointName { get; set; }

        Type IRegistrationRootAttribute.HandlerType => typeof(RegisterStatelessServiceHandler);

        IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> IRegistrationBuilderAttribute.Build<TLimit, TActivatorData, TRegistrationStyle>(Type type, IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            if (DefaultEndpointName != null && builder is IRegistrationBuilder<TLimit, ReflectionActivatorData, TRegistrationStyle> b)
                return (IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>)b.WithParameter(TypedParameter.From(new DefaultServiceEndpoint(DefaultEndpointName)));
            else
                return builder;
        }

    }

}
