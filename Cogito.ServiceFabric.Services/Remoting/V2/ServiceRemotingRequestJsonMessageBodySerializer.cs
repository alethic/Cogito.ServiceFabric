using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Messaging;

using Newtonsoft.Json;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class ServiceRemotingRequestJsonMessageBodySerializer : IServiceRemotingRequestMessageBodySerializer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <param name="requestWrappedTypes"></param>
        /// <param name="requestBodyTypes"></param>
        public ServiceRemotingRequestJsonMessageBodySerializer(
            Type serviceInterfaceType,
            IEnumerable<Type> requestWrappedTypes,
            IEnumerable<Type> requestBodyTypes)
        {

        }

        public IOutgoingMessageBody Serialize(IServiceRemotingRequestMessageBody serviceRemotingRequestMessageBody)
        {
            if (serviceRemotingRequestMessageBody == null)
                return null;

            using (var writeStream = new MemoryStream())
            {
                using (var jsonWriter = new JsonTextWriter(new StreamWriter(writeStream)))
                    JsonSerializerConfig.Serializer.Serialize(jsonWriter, serviceRemotingRequestMessageBody);

                return new OutgoingMessageBody(new[] { new ArraySegment<byte>(writeStream.ToArray()) });
            }
        }

        public IServiceRemotingRequestMessageBody Deserialize(IIncomingMessageBody messageBody)
        {
            using (var reader = new JsonTextReader(new StreamReader(messageBody.GetReceivedBuffer())))
                return JsonSerializerConfig.Serializer.Deserialize<JsonRemotingRequestBody>(reader);
        }

    }

}
