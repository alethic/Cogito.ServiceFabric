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

        static readonly JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            DateParseHandling = DateParseHandling.None,
        });

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <param name="parameterInfo"></param>
        public ServiceRemotingRequestJsonMessageBodySerializer(
            Type serviceInterfaceType,
            IEnumerable<Type> parameterInfo)
        {

        }

        public OutgoingMessageBody Serialize(IServiceRemotingRequestMessageBody serviceRemotingRequestMessageBody)
        {
            if (serviceRemotingRequestMessageBody == null)
                return null;

            using (var writeStream = new MemoryStream())
            {
                // write body to stream
                using (var jsonWriter = new JsonTextWriter(new StreamWriter(writeStream)))
                    serializer.Serialize(jsonWriter, serviceRemotingRequestMessageBody);

                return new OutgoingMessageBody(new[] { new ArraySegment<byte>(writeStream.ToArray()) });
            }
        }

        public IServiceRemotingRequestMessageBody Deserialize(IncomingMessageBody messageBody)
        {
            using (var sr = new StreamReader(messageBody.GetReceivedBuffer()))
            using (var reader = new JsonTextReader(sr))
                return serializer.Deserialize<JsonRemotingRequestBody>(reader);
        }

    }

}
