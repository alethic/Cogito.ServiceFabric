using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric
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
            Contract.Requires<ArgumentNullException>(reference != null);

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
            Contract.Requires<ArgumentNullException>(references != null);

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
            Contract.Requires<ArgumentNullException>(references != null);

            return references.Select(i => i.Bind<TActor>());
        }

        /// <summary>
        /// Gets the <see cref="ActorReference"/>s for the <see cref="IActor"/>s.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static ActorReference[] GetActorReferences(this IActor[] actors)
        {
            Contract.Requires<ArgumentNullException>(actors != null);

            return actors.Select(i => i.GetActorReference()).ToArray();
        }

        /// <summary>
        /// Gets the <see cref="ActorReference"/>s for the <see cref="IActor"/>s.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static IEnumerable<ActorReference> GetActorReferences(this IEnumerable<IActor> actors)
        {
            Contract.Requires<ArgumentNullException>(actors != null);

            return actors.Select(i => i.GetActorReference());
        }

    }

}
