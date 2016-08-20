using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    public interface ITestActor :
        IActor
    {

        Task IncrementThing();

        Task Connect();

    }

}
