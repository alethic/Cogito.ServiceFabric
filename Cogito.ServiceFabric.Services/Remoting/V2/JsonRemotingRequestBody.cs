using System;
using System.Collections.Generic;

using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class JsonRemotingRequestBody : IServiceRemotingRequestMessageBody
    {

        readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public void SetParameter(int position, string parameName, object parameter)
        {
            parameters[parameName] = parameter;
        }

        public object GetParameter(int position, string parameName, Type paramType)
        {
            return parameters[parameName];
        }

    }

}
