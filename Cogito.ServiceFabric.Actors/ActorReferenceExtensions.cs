using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.ServiceFabric.Actors;

namespace Cogito.ServiceFabric.Actors
{

    /// <summary>
    /// Provides extension methods for working with <see cref="ActorReference"/> instances.
    /// </summary>
    public static class ActorReferenceExtensions
    {

        /// <summary>
        /// Creates an <see cref="ActorProxy"/> of the specified type.
        /// </summary>
        /// <typeparam name="TActor"></typeparam>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static TActor Bind<TActor>(this ActorReference reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            return (TActor)reference.Bind(typeof(TActor));
        }

        /// <summary>
        /// Creates an <see cref="ActorProxy"/> of the specified type.
        /// </summary>
        /// <typeparam name="TActor"></typeparam>
        /// <param name="references"></param>
        /// <returns></returns>
        public static TActor[] Bind<TActor>(this ActorReference[] references)
        {
            if (references == null)
                throw new ArgumentNullException(nameof(references));

            return references.Select(i => i.Bind<TActor>()).ToArray();
        }

        /// <summary>
        /// Creates an <see cref="ActorProxy"/> of the specified type.
        /// </summary>
        /// <typeparam name="TActor"></typeparam>
        /// <param name="references"></param>
        /// <returns></returns>
        public static IEnumerable<TActor> Bind<TActor>(this IEnumerable<ActorReference> references)
        {
            if (references == null)
                throw new ArgumentNullException(nameof(references));

            return references.Select(i => i.Bind<TActor>());
        }

        /// <summary>
        /// Gets the <see cref="ActorReference"/>s for the <see cref="IActor"/>s.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static ActorReference[] GetActorReferences(this IActor[] actors)
        {
            if (actors == null)
                throw new ArgumentNullException(nameof(actors));

            return actors.Select(i => i.GetActorReference()).ToArray();
        }

        /// <summary>
        /// Gets the <see cref="ActorReference"/>s for the <see cref="IActor"/>s.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static IEnumerable<ActorReference> GetActorReferences(this IEnumerable<IActor> actors)
        {
            if (actors == null)
                throw new ArgumentNullException(nameof(actors));

            return actors.Select(i => i.GetActorReference());
        }

    }

}
