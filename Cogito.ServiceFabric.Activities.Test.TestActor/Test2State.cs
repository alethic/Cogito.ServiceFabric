using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces;

namespace Cogito.ServiceFabric.Activities.Test.TestActor
{

    [DataContract]
    public class Test2State
    {

        [DataMember]
        public List<ITest> Others { get; set; } = new List<ITest>();

        [DataMember]
        public XElement Element { get; set; }

    }

}
