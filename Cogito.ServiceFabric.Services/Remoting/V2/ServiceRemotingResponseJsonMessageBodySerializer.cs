using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Messaging;

using Newtonsoft.Json;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    class ServiceRemotingResponseJsonMessageBodySerializer : IServiceRemotingResponseMessageBodySerializer
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
        public ServiceRemotingResponseJsonMessageBodySerializer(
            Type serviceInterfaceType,
            IEnumerable<Type> parameterInfo)
        {

        }

        public OutgoingMessageBody Serialize(IServiceRemotingResponseMessageBody responseMessageBody)
        {
            using (var stream = new MemoryStream())
            {
                // serialize to stream
                using (var writer = new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8)))
                    serializer.Serialize(writer, responseMessageBody);

                var segment = new ArraySegment<byte>(stream.ToArray());
                var list = new List<ArraySegment<byte>> { segment };
                return new OutgoingMessageBody(list);
            }
        }

        public IServiceRemotingResponseMessageBody Deserialize(IncomingMessageBody messageBody)
        {
            using (var sr = new StreamReader(messageBody.GetReceivedBuffer()))
            using (var reader = new JsonTextReader(sr))
                return serializer.Deserialize<JsonRemotingResponseBody>(reader);
        }

    }

}
