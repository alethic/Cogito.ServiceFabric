using System;
using System.Diagnostics.Tracing;
using System.Fabric;

namespace Cogito.ServiceFabric.Actors
{

    [EventSource(Name ="Cogito-ServiceFabric")]
    sealed class EventSource :
        System.Diagnostics.Tracing.EventSource
    {
         
        public static EventSource Current = new EventSource();

        #region Informational Messages

        /// <summary>
        /// Logs a basic informational message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        public void Message(string message, params object[] args)
        {
            if (IsEnabled())
                Message(string.Format(message, args));
        }

        const int MessageEventId = 1;

        /// <summary>
        /// Logs a basic informational message.
        /// </summary>
        /// <param name="message"></param>
        [Event(MessageEventId, Level = EventLevel.Informational, Message = "{0}")]
        public void Message(string message)
        {
            if (IsEnabled())
                WriteEvent(
                    MessageEventId,
                    message);
        }

        /// <summary>
        /// Logs a basic informational message from a service.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        public void ServiceMessage(StatelessService service, string message, params object[] args)
        {
            if (IsEnabled())
                ServiceMessage(
                    service.Context.ServiceName.ToString(),
                    service.Context.ServiceTypeName,
                    service.Context.InstanceId,
                    service.Context.PartitionId,
                    service.Context.CodePackageActivationContext.ApplicationName,
                    service.Context.CodePackageActivationContext.ApplicationTypeName,
                    FabricRuntime.GetNodeContext().NodeName,
                    string.Format(message, args));
        }

        /// <summary>
        /// Logs a basic informational message from a service.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        public void ServiceMessage(StatefulService service, string message, params object[] args)
        {
            if (IsEnabled())
                ServiceMessage(
                    service.Context.ServiceName.ToString(),
                    service.Context.ServiceTypeName,
                    service.Context.ReplicaId,
                    service.Context.PartitionId,
                    service.Context.CodePackageActivationContext.ApplicationName,
                    service.Context.CodePackageActivationContext.ApplicationTypeName,
                    FabricRuntime.GetNodeContext().NodeName,
                    string.Format(message, args));
        }

        const int ServiceMessageEventId = 2;

        /// <summary>
        /// Logs a basic informational message from a service.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="partitionId"></param>
        /// <param name="applicationName"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="nodeName"></param>
        /// <param name="message"></param>
        [Event(ServiceMessageEventId, Level = EventLevel.Informational, Message = "{7}")]
        void ServiceMessage(
            string serviceName,
            string serviceTypeName,
            long replicaOrInstanceId,
            Guid partitionId,
            string applicationName,
            string applicationTypeName,
            string nodeName,
            string message)
        {
            WriteEvent(
                ServiceMessageEventId,
                serviceName,
                serviceTypeName,
                replicaOrInstanceId,
                partitionId,
                applicationName,
                applicationTypeName,
                nodeName,
                message);
        }

        #endregion

    }

}
