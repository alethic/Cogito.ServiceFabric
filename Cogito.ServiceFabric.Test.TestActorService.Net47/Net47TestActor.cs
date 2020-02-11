using System.Threading.Tasks;

using Cogito.ServiceFabric.Actors.Autofac;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Test.TestActorService.Net47
{

    [RegisterActor]
    [StatePersistence(StatePersistence.Volatile)]
    public class Net47TestActor : Actor, INet47TestActor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="actorService"></param>
        /// <param name="actorId"></param>
        public Net47TestActor(ActorService actorService, ActorId actorId) :
            base(actorService, actorId)
        {

        }

        protected override Task OnActivateAsync()
        {
            return StateManager.TryAddStateAsync("count", 0);
        }

    }

}
