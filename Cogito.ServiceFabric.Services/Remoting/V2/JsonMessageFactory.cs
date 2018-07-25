using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class JsonMessageFactory : IServiceRemotingMessageBodyFactory
    {

        public IServiceRemotingRequestMessageBody CreateRequest(string interfaceName, string methodName, int numberOfParameters)
        {
            return new JsonRemotingRequestBody();
        }

        public IServiceRemotingResponseMessageBody CreateResponse(string interfaceName, string methodName)
        {
            return new JsonRemotingResponseBody();
        }

    }

}
