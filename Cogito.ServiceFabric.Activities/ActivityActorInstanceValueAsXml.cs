using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Describes a serialized object type.
    /// </summary>
    [DataContract]
    class ActivityActorInstanceValueAsXml
    {

        /// <summary>
        /// Serialized data as XML.
        /// </summary>
        [DataMember]
        public XElement Value { get; set; }

    }

}
