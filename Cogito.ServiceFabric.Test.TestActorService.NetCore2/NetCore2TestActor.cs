using System.Threading.Tasks;

using Cogito.ServiceFabric.Actors.Autofac;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Test.TestActorService.NetCore2
{

    [RegisterActor]
    [StatePersistence(StatePersistence.Volatile)]
    public class NetCore2TestActor : Actor, INetCore2TestActor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="actorService"></param>
        /// <param name="actorId"></param>
        public NetCore2TestActor(ActorService actorService, ActorId actorId) :
            base(actorService, actorId)
        {

        }

        protected override Task OnActivateAsync()
        {
            return StateManager.TryAddStateAsync("count", 0);
        }

    }

}
