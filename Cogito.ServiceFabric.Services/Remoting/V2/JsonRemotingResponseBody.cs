using System;

using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class JsonRemotingResponseBody : IServiceRemotingResponseMessageBody
    {

        object value;

        public void Set(object response)
        {
            value = response;
        }

        public object Get(Type paramType)
        {
            return value;
        }

    }

}
