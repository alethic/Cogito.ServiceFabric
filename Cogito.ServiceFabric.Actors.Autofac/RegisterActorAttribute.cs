using System;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Actors.Autofac
{

    /// <summary>
    /// Registers an actor class.
    /// </summary>
    public class RegisterActorAttribute : Attribute, IRegistrationRootAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RegisterActorAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="actorServiceType"></param>
        public RegisterActorAttribute(Type actorServiceType)
        {
            ActorServiceType = actorServiceType;
        }

        /// <summary>
        /// Specifies the type of the actor service to use.
        /// </summary>
        public Type ActorServiceType { get; set; }

        /// <summary>
        /// Specifies the state provider type for the actor to use.
        /// </summary>
        public Type StateProviderType { get; set; }

        /// <summary>
        /// Specifies the tag to apply to the lifetime scope that hosts the actor.
        /// </summary>
        public object LifetimeScopeTag { get; set; }

        Type IRegistrationRootAttribute.HandlerType => typeof(RegisterActorHandler);

    }

}
