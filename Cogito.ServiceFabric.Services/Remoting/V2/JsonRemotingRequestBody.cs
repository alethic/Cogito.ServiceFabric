using System;
using System.Collections.Generic;

using Microsoft.ServiceFabric.Services.Remoting.V2;
using Newtonsoft.Json;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class JsonRemotingRequestBody : IServiceRemotingRequestMessageBody
    {

        [JsonProperty("Parameters")]
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        public void SetParameter(int position, string paramName, object parameter)
        {
            Parameters[paramName] = parameter;
        }

        public object GetParameter(int position, string paramName, Type paramType)
        {
            return Parameters[paramName];
        }

    }

}
