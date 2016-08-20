using System.Runtime.Serialization;

namespace Cogito.ServiceFabric.Test.Web.Service
{

    [DataContract]
    class TestActorState
    {

        [DataMember]
        public int Thing { get; set; }

    }

}
