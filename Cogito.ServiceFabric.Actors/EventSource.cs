using System;
using System.Diagnostics.Tracing;

using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Actors
{

    [EventSource(Name ="Cogito-ServiceFabric-Actors")]
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

        /// <summary>
        /// Logs a basic informational message from an actor.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        public void ActorMessage(Actor actor, string message, params object[] args)
        {
            if (IsEnabled() &&
                actor.Id != null &&
                actor.ActorService != null &&
                actor.ActorService.Context != null &&
                actor.ActorService.Context.CodePackageActivationContext != null)
            {
                ActorMessage(
                    actor.GetType().ToString(),
                    actor.Id.ToString(),
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                    actor.ActorService.Context.ServiceTypeName,
                    actor.ActorService.Context.ServiceName.ToString(),
                    actor.ActorService.Context.PartitionId,
                    actor.ActorService.Context.ReplicaId,
                    actor.ActorService.Context.NodeContext.NodeName,
                    string.Format(message, args));
            }
        }

        /// <summary>
        /// Logs a basic informational message from an actor.
        /// </summary>
        const int ActorMessageEventId = 3;
        [Event(ActorMessageEventId, Level = EventLevel.Informational, Message = "{9}")]
        void ActorMessage(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            string message)
        {
            WriteEvent(
                ActorMessageEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                message);
        }

        #endregion

        [NonEvent]
        public void ActorMethodPre(Actor actor, ActorMethodContext context)
        {
            if (IsEnabled() &&
                actor.Id != null &&
                actor.ActorService != null &&
                actor.ActorService.Context != null &&
                actor.ActorService.Context.CodePackageActivationContext != null)
            {
                ActorMethodPre(
                    actor.GetType().ToString(),
                    actor.Id.ToString(),
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                    actor.ActorService.Context.ServiceTypeName,
                    actor.ActorService.Context.ServiceName.ToString(),
                    actor.ActorService.Context.PartitionId,
                    actor.ActorService.Context.ReplicaId,
                    actor.ActorService.Context.NodeContext.NodeName,
                    context.CallType,
                    context.MethodName);
            }
        }

        const int ActorMethodPreEventId = 4;
        [Event(ActorMethodPreEventId, Level = EventLevel.Informational, Message = "{10} {9}")]
        void ActorMethodPre(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            ActorCallType callType,
            string methodName)
        {
            WriteEvent(
                ActorMethodPreEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                callType,
                methodName);
        }

        [NonEvent]
        public void ActorMethodPost(Actor actor, ActorMethodContext context)
        {
            if (IsEnabled() &&
                actor.Id != null &&
                actor.ActorService != null &&
                actor.ActorService.Context != null &&
                actor.ActorService.Context.CodePackageActivationContext != null)
            {
                ActorMethodPost(
                    actor.GetType().ToString(),
                    actor.Id.ToString(),
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                    actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                    actor.ActorService.Context.ServiceTypeName,
                    actor.ActorService.Context.ServiceName.ToString(),
                    actor.ActorService.Context.PartitionId,
                    actor.ActorService.Context.ReplicaId,
                    actor.ActorService.Context.NodeContext.NodeName,
                    context.CallType,
                    context.MethodName);
            }
        }

        const int ActorMethodPostEventId = 5;
        [Event(ActorMethodPostEventId, Level = EventLevel.Informational, Message = "{10} {9}")]
        void ActorMethodPost(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            ActorCallType callType,
            string methodName)
        {
            WriteEvent(
                ActorMethodPostEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                callType,
                methodName);
        }

    }

}
