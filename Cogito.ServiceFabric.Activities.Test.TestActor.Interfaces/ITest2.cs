using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;

namespace Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces
{

    public interface ITest2 : 
        IActor
    {

        Task CallMe1(ITest test1);

        Task CallMe2();

        Task Start();

    }

}
