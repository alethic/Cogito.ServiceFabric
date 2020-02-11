using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac;
using Autofac.Builder;
using Autofac.Integration.ServiceFabric;

using Cogito.Autofac;

using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Actors.Autofac
{

    /// <summary>
    /// Supports the <see cref="RegisterActorAttribute"/>.
    /// </summary>
    class RegisterActorHandler : RegisterTypeHandler
    {

        static readonly MethodInfo RegisterActorMethod = typeof(AutofacActorExtensions).GetMethod(nameof(AutofacActorExtensions.RegisterActor));

        /// <summary>
        /// Invoked the generate RegisterActor method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="actorType"></param>
        /// <param name="actorServiceType"></param>
        /// <param name="stateManagerFactory"></param>
        /// <param name="stateProvider"></param>
        /// <param name="settings"></param>
        /// <param name="lifetimeScopeTag"></param>
        /// <returns></returns>
        static IRegistrationBuilder<ActorBase, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterActor(ContainerBuilder builder, Type actorType, Type actorServiceType = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null, object lifetimeScopeTag = null)
        {
            return (IRegistrationBuilder<ActorBase, ConcreteReflectionActivatorData, SingleRegistrationStyle>)RegisterActorMethod.MakeGenericMethod(new[] { actorType }).Invoke(null, new object[] { builder, actorServiceType, stateManagerFactory, stateProvider, settings, lifetimeScopeTag });
        }

        protected override void RegisterCore(ContainerBuilder builder, Type type, IRegistrationRootAttribute attribute, IEnumerable<IRegistrationBuilderAttribute> builders)
        {
            if (attribute is RegisterActorAttribute a)
                ApplyBuilders(type, RegisterActor(builder, type, a.ActorServiceType, null, null, null, a.LifetimeScopeTag), attribute, builders);
        }

    }

}
