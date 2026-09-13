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
    /// Supports the <see cref="RegisterStatefulServiceAttribute"/>.
    /// </summary>
    class RegisterStatefulServiceHandler : RegisterTypeHandler
    {

        static readonly MethodInfo RegisterStatefulServiceMethod = typeof(AutofacServiceExtensions).GetMethod(nameof(AutofacServiceExtensions.RegisterStatefulService));

        /// <summary>
        /// Invoke the generate RegisterStatefulService method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="lifetimeScopeTag"></param>
        /// <returns></returns>
        static IRegistrationBuilder<Microsoft.ServiceFabric.Services.Runtime.StatefulService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterStatefulService(ContainerBuilder builder, Type serviceType, string serviceTypeName = null, object lifetimeScopeTag = null)
        {
            return (IRegistrationBuilder<Microsoft.ServiceFabric.Services.Runtime.StatefulService, ConcreteReflectionActivatorData, SingleRegistrationStyle>)RegisterStatefulServiceMethod.MakeGenericMethod(new[] { serviceType }).Invoke(null, new object[] { builder, serviceTypeName, lifetimeScopeTag });
        }

        protected override void RegisterCore(ContainerBuilder builder, Type type, IRegistrationRootAttribute attribute, IEnumerable<IRegistrationBuilderAttribute> builders)
        {
            if (attribute is RegisterStatefulServiceAttribute a)
                ApplyBuilders(type, RegisterStatefulService(builder, type, a.ServiceTypeName ?? type.FullName, a.LifetimeScopeTag), attribute, builders);
        }

    }

}
