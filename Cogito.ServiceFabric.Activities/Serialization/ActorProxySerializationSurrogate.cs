using System;
using System.Linq;
using System.Runtime.Serialization;

using Cogito.Reflection;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Activities.Serialization
{

    /// <summary>
    /// Replaces a <see cref="ActorProxy"/> with a <see cref="ActorProxyReference"/>.
    /// </summary>
    class ActorProxySerializationSurrogate :
        ISerializationSurrogate
    {

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var p = (IActor)(ActorProxy)obj;
            var t = p.GetType().GetAssignableTypes().FirstOrDefault(i => i.IsInterface && typeof(IActor).IsAssignableFrom(i));
            if (t == null)
                throw new SerializationException("Unable to find IActor interface type for ActorProxy.");

            info.SetType(typeof(ActorProxyReference));
            info.AddValue(nameof(ActorProxyReference.ActorType), t);
            info.AddValue(nameof(ActorProxyReference.ActorReference), p.GetActorReference());
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            throw new NotSupportedException();
        }

    }

}
