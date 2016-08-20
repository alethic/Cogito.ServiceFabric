using System;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Serializable object that holds the activity state.
    /// </summary>
    [DataContract]
    [KnownType(typeof(XName))]
    [KnownType(typeof(ActivityActorInstanceValueAsXml))]
    [KnownType(typeof(ReadOnlyCollection<BookmarkInfo>))]
    class ActivityActorState
    {

        /// <summary>
        /// Owner ID.
        /// </summary>
        [DataMember]
        public Guid InstanceOwnerId { get; set; }

        /// <summary>
        /// Instance ID.
        /// </summary>
        [DataMember]
        public Guid InstanceId { get; set; }

        /// <summary>
        /// State of the instance.
        /// </summary>
        [DataMember]
        public InstanceState InstanceState { get; set; }

        /// <summary>
        /// Data of the instance.
        /// </summary>
        [DataMember]
        public KeyValuePair<XName, object>[] InstanceData { get; set; }

        /// <summary>
        /// Metadata of the instance.
        /// </summary>
        [DataMember]
        public KeyValuePair<XName, object>[] InstanceMetadata { get; set; }

    }

}
