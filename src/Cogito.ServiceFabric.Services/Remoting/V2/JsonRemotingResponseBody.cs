using System;

using Microsoft.ServiceFabric.Services.Remoting.V2;

using Newtonsoft.Json;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class JsonRemotingResponseBody : IServiceRemotingResponseMessageBody
    {

        [JsonProperty("Value")]
        public object Value { get; set; }

        public void Set(object response)
        {
            Value = response;
        }

        public object Get(Type paramType)
        {
            return Value;
        }

    }

}
