using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    public interface IOwinStatefulService :
        IService
    {

        Task<int> Ping();

    }

}
