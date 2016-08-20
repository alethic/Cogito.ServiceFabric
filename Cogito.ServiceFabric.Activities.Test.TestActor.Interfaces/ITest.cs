using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;

namespace Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces
{

    public interface ITest :
        IActor
    {

        Task CallMe();

        Task CallMeBack(ITest2 from);

        Task Start();

    }

}
