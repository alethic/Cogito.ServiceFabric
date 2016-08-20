using System;
using System.Runtime.Serialization;

using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Activities.Serialization
{

    public class ActivitySurrogateSelector :
        ISurrogateSelector
    {

        readonly ActorProxySerializationSurrogate actorProxySurrogate;
        readonly ActorProxyReferenceSerializationSurrogate actorProxyReferenceSurrogate;
        ISurrogateSelector next;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ActivitySurrogateSelector()
        {
            this.actorProxySurrogate = new ActorProxySerializationSurrogate();
            this.actorProxyReferenceSurrogate = new ActorProxyReferenceSerializationSurrogate();
        }

        public void ChainSelector(ISurrogateSelector selector)
        {
            next = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return next;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = null;

            if (typeof(ActorProxy).IsAssignableFrom(type))
                return actorProxySurrogate;

            if (typeof(ActorProxyReference).IsAssignableFrom(type))
                return actorProxyReferenceSurrogate;

            selector = next;
            return null;
        }

    }

}
