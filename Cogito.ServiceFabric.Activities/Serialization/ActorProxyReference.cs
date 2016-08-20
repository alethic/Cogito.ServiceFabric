using System;
using System.Runtime.Serialization;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Activities.Serialization
{

    /// <summary>
    /// Encapsulates an <see cref="ActorProxy"/> during <see cref="NetDataContractSerializer"/> serialization.
    /// </summary>
    [DataContract]
    public class ActorProxyReference
    {

        /// <summary>
        /// Type of the actor.
        /// </summary>
        [DataMember]
        public Type ActorType { get; set; }

        /// <summary>
        /// Reference to the actor.
        /// </summary>
        [DataMember]
        public ActorReference ActorReference { get; set; }

    }

}
