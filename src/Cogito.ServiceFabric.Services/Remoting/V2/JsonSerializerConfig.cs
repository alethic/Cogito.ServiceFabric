using Newtonsoft.Json;

namespace Cogito.ServiceFabric.Services.Remoting.V2
{

    static class JsonSerializerConfig
    {

        public static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            DateParseHandling = DateParseHandling.None,
        });

    }

}
