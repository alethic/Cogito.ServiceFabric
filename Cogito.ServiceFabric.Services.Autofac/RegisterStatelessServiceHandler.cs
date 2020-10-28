using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac;
using Autofac.Builder;
using Autofac.Integration.ServiceFabric;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Services.Autofac
{

    /// <summary>
    /// Supports the <see cref="RegisterStatelessServiceAttribute"/>.
    /// </summary>
    class RegisterStatelessServiceHandler : RegisterTypeHandler
    {

        static readonly MethodInfo RegisterStatelessServiceMethod = typeof(AutofacServiceExtensions).GetMethod(nameof(AutofacServiceExtensions.RegisterStatelessService));

        /// <summary>
        /// Invoke the generate RegisterStatelessService method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="lifetimeScopeTag"></param>
        /// <returns></returns>
        static IRegistrationBuilder<Microsoft.ServiceFabric.Services.Runtime.StatelessService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterStatelessService(ContainerBuilder builder, Type serviceType, string serviceTypeName = null, object lifetimeScopeTag = null)
        {
            return (IRegistrationBuilder<Microsoft.ServiceFabric.Services.Runtime.StatelessService, ConcreteReflectionActivatorData, SingleRegistrationStyle>)RegisterStatelessServiceMethod.MakeGenericMethod(new[] { serviceType }).Invoke(null, new object[] { builder, serviceTypeName, lifetimeScopeTag });
        }

        protected override void RegisterCore(ContainerBuilder builder, Type type, IRegistrationRootAttribute attribute, IEnumerable<IRegistrationBuilderAttribute> builders)
        {
            if (attribute is RegisterStatelessServiceAttribute a)
                ApplyBuilders(type, RegisterStatelessService(builder, type, a.ServiceTypeName ?? type.FullName, a.LifetimeScopeTag), attribute, builders);
        }

    }

}
