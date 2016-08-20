using System;
using System.Runtime.Serialization;

using Cogito.Serialization;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Activities.Serialization
{

    /// <summary>
    /// Replaces a <see cref="ActorProxyReference"/> with an <see cref="ActorProxy"/>.
    /// </summary>
    class ActorProxyReferenceSerializationSurrogate :
        ISerializationSurrogate
    {

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            throw new NotSupportedException();
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var t = info.GetValue<Type>(nameof(ActorProxyReference.ActorType));
            var r = info.GetValue<ActorReference>(nameof(ActorProxyReference.ActorReference));
            return r.Bind(t);
        }

    }

}
