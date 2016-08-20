using System;
using System.Activities.Tracking;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.Fabric;

namespace Cogito.ServiceFabric.Activities
{

    sealed partial class ActivityActorEventSource
    {

        const int ActivityScheduledVerboseEventId = 110;
        const int ActivityScheduledInfoEventId = 111;
        const int ActivityScheduledWarningEventId = 112;
        const int ActivityScheduledErrorEventId = 113;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void ActivityScheduled(IActivityActorInternal actor, ActivityScheduledRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.ActivityScheduled))
                {
                    ActivityScheduledVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.ActivityScheduled))
                {
                    ActivityScheduledInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.ActivityScheduled))
                {
                    ActivityScheduledWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.ActivityScheduled))
                {
                    ActivityScheduledError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            ActivityScheduledVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.ActivityScheduled)]
        internal void ActivityScheduledVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                ActivityScheduledVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            ActivityScheduledInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.ActivityScheduled)]
        internal void ActivityScheduledInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                ActivityScheduledInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            ActivityScheduledWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.ActivityScheduled)]
        internal void ActivityScheduledWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                ActivityScheduledWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            ActivityScheduledErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.ActivityScheduled)]
        internal void ActivityScheduledError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                ActivityScheduledErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int ActivityStateVerboseEventId = 120;
        const int ActivityStateInfoEventId = 121;
        const int ActivityStateWarningEventId = 122;
        const int ActivityStateErrorEventId = 123;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void ActivityState(IActivityActorInternal actor, ActivityStateRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.ActivityState))
                {
                    ActivityStateVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Arguments)) ?? string.Empty,
                        (PrepareDictionary(record.Variables)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.ActivityState))
                {
                    ActivityStateInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Arguments)) ?? string.Empty,
                        (PrepareDictionary(record.Variables)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.ActivityState))
                {
                    ActivityStateWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Arguments)) ?? string.Empty,
                        (PrepareDictionary(record.Variables)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.ActivityState))
                {
                    ActivityStateError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Arguments)) ?? string.Empty,
                        (PrepareDictionary(record.Variables)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="arguments"></param>
        /// <param name="variables"></param>
        /// <param name="message"></param>
        [Event(
            ActivityStateVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{20}",
            Keywords = Keywords.ActivityState)]
        internal void ActivityStateVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string arguments,
            string variables,
            string message)
        {
            WriteEvent(
                ActivityStateVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                arguments,
                variables,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="arguments"></param>
        /// <param name="variables"></param>
        /// <param name="message"></param>
        [Event(
            ActivityStateInfoEventId,
            Level = EventLevel.Informational,
            Message = "{20}",
            Keywords = Keywords.ActivityState)]
        internal void ActivityStateInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string arguments,
            string variables,
            string message)
        {
            WriteEvent(
                ActivityStateInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                arguments,
                variables,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="arguments"></param>
        /// <param name="variables"></param>
        /// <param name="message"></param>
        [Event(
            ActivityStateWarningEventId,
            Level = EventLevel.Warning,
            Message = "{20}",
            Keywords = Keywords.ActivityState)]
        internal void ActivityStateWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string arguments,
            string variables,
            string message)
        {
            WriteEvent(
                ActivityStateWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                arguments,
                variables,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="arguments"></param>
        /// <param name="variables"></param>
        /// <param name="message"></param>
        [Event(
            ActivityStateErrorEventId,
            Level = EventLevel.Error,
            Message = "{20}",
            Keywords = Keywords.ActivityState)]
        internal void ActivityStateError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string arguments,
            string variables,
            string message)
        {
            WriteEvent(
                ActivityStateErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                arguments,
                variables,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int BookmarkResumptionVerboseEventId = 130;
        const int BookmarkResumptionInfoEventId = 131;
        const int BookmarkResumptionWarningEventId = 132;
        const int BookmarkResumptionErrorEventId = 133;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void BookmarkResumption(IActivityActorInternal actor, BookmarkResumptionRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.BookmarkResumption))
                {
                    BookmarkResumptionVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.BookmarkName) ?? string.Empty,
                        record.BookmarkScope,
                        (null) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.BookmarkResumption))
                {
                    BookmarkResumptionInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.BookmarkName) ?? string.Empty,
                        record.BookmarkScope,
                        (null) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.BookmarkResumption))
                {
                    BookmarkResumptionWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.BookmarkName) ?? string.Empty,
                        record.BookmarkScope,
                        (null) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.BookmarkResumption))
                {
                    BookmarkResumptionError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.BookmarkName) ?? string.Empty,
                        record.BookmarkScope,
                        (null) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="bookmarkName"></param>
        /// <param name="bookmarkScope"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        [Event(
            BookmarkResumptionVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{16}",
            Keywords = Keywords.BookmarkResumption)]
        internal void BookmarkResumptionVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string bookmarkName,
            Guid bookmarkScope,
            string payload,
            string message)
        {
            WriteEvent(
                BookmarkResumptionVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                bookmarkName,
                bookmarkScope,
                payload,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="bookmarkName"></param>
        /// <param name="bookmarkScope"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        [Event(
            BookmarkResumptionInfoEventId,
            Level = EventLevel.Informational,
            Message = "{16}",
            Keywords = Keywords.BookmarkResumption)]
        internal void BookmarkResumptionInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string bookmarkName,
            Guid bookmarkScope,
            string payload,
            string message)
        {
            WriteEvent(
                BookmarkResumptionInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                bookmarkName,
                bookmarkScope,
                payload,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="bookmarkName"></param>
        /// <param name="bookmarkScope"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        [Event(
            BookmarkResumptionWarningEventId,
            Level = EventLevel.Warning,
            Message = "{16}",
            Keywords = Keywords.BookmarkResumption)]
        internal void BookmarkResumptionWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string bookmarkName,
            Guid bookmarkScope,
            string payload,
            string message)
        {
            WriteEvent(
                BookmarkResumptionWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                bookmarkName,
                bookmarkScope,
                payload,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="bookmarkName"></param>
        /// <param name="bookmarkScope"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        [Event(
            BookmarkResumptionErrorEventId,
            Level = EventLevel.Error,
            Message = "{16}",
            Keywords = Keywords.BookmarkResumption)]
        internal void BookmarkResumptionError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string bookmarkName,
            Guid bookmarkScope,
            string payload,
            string message)
        {
            WriteEvent(
                BookmarkResumptionErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                bookmarkName,
                bookmarkScope,
                payload,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int CancelRequestedVerboseEventId = 140;
        const int CancelRequestedInfoEventId = 141;
        const int CancelRequestedWarningEventId = 142;
        const int CancelRequestedErrorEventId = 143;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void CancelRequested(IActivityActorInternal actor, CancelRequestedRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.CancelRequested))
                {
                    CancelRequestedVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.CancelRequested))
                {
                    CancelRequestedInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.CancelRequested))
                {
                    CancelRequestedWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.CancelRequested))
                {
                    CancelRequestedError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            CancelRequestedVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.CancelRequested)]
        internal void CancelRequestedVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                CancelRequestedVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            CancelRequestedInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.CancelRequested)]
        internal void CancelRequestedInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                CancelRequestedInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            CancelRequestedWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.CancelRequested)]
        internal void CancelRequestedWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                CancelRequestedWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="message"></param>
        [Event(
            CancelRequestedErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.CancelRequested)]
        internal void CancelRequestedError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string message)
        {
            WriteEvent(
                CancelRequestedErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int FaultPropagationVerboseEventId = 150;
        const int FaultPropagationInfoEventId = 151;
        const int FaultPropagationWarningEventId = 152;
        const int FaultPropagationErrorEventId = 153;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void FaultPropagation(IActivityActorInternal actor, FaultPropagationRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.FaultPropagation))
                {
                    FaultPropagationVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Fault?.ToString()) ?? string.Empty,
                        (record.Fault?.Message) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.FaultHandler?.Name) ?? string.Empty,
                        (record.FaultHandler?.Id) ?? string.Empty,
                        (record.FaultHandler?.InstanceId) ?? string.Empty,
                        (record.FaultHandler?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.FaultPropagation))
                {
                    FaultPropagationInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Fault?.ToString()) ?? string.Empty,
                        (record.Fault?.Message) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.FaultHandler?.Name) ?? string.Empty,
                        (record.FaultHandler?.Id) ?? string.Empty,
                        (record.FaultHandler?.InstanceId) ?? string.Empty,
                        (record.FaultHandler?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.FaultPropagation))
                {
                    FaultPropagationWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Fault?.ToString()) ?? string.Empty,
                        (record.Fault?.Message) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.FaultHandler?.Name) ?? string.Empty,
                        (record.FaultHandler?.Id) ?? string.Empty,
                        (record.FaultHandler?.InstanceId) ?? string.Empty,
                        (record.FaultHandler?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.FaultPropagation))
                {
                    FaultPropagationError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Fault?.ToString()) ?? string.Empty,
                        (record.Fault?.Message) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.FaultHandler?.Name) ?? string.Empty,
                        (record.FaultHandler?.Id) ?? string.Empty,
                        (record.FaultHandler?.InstanceId) ?? string.Empty,
                        (record.FaultHandler?.TypeName) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="fault"></param>
        /// <param name="faultMessage"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="faultHandlerName"></param>
        /// <param name="faultHandlerId"></param>
        /// <param name="faultHandlerInstanceId"></param>
        /// <param name="faultHandlerTypeName"></param>
        /// <param name="message"></param>
        [Event(
            FaultPropagationVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{23}",
            Keywords = Keywords.FaultPropagation)]
        internal void FaultPropagationVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string fault,
            string faultMessage,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string faultHandlerName,
            string faultHandlerId,
            string faultHandlerInstanceId,
            string faultHandlerTypeName,
            string message)
        {
            WriteEvent(
                FaultPropagationVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                fault,
                faultMessage,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                faultHandlerName,
                faultHandlerId,
                faultHandlerInstanceId,
                faultHandlerTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="fault"></param>
        /// <param name="faultMessage"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="faultHandlerName"></param>
        /// <param name="faultHandlerId"></param>
        /// <param name="faultHandlerInstanceId"></param>
        /// <param name="faultHandlerTypeName"></param>
        /// <param name="message"></param>
        [Event(
            FaultPropagationInfoEventId,
            Level = EventLevel.Informational,
            Message = "{23}",
            Keywords = Keywords.FaultPropagation)]
        internal void FaultPropagationInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string fault,
            string faultMessage,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string faultHandlerName,
            string faultHandlerId,
            string faultHandlerInstanceId,
            string faultHandlerTypeName,
            string message)
        {
            WriteEvent(
                FaultPropagationInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                fault,
                faultMessage,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                faultHandlerName,
                faultHandlerId,
                faultHandlerInstanceId,
                faultHandlerTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="fault"></param>
        /// <param name="faultMessage"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="faultHandlerName"></param>
        /// <param name="faultHandlerId"></param>
        /// <param name="faultHandlerInstanceId"></param>
        /// <param name="faultHandlerTypeName"></param>
        /// <param name="message"></param>
        [Event(
            FaultPropagationWarningEventId,
            Level = EventLevel.Warning,
            Message = "{23}",
            Keywords = Keywords.FaultPropagation)]
        internal void FaultPropagationWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string fault,
            string faultMessage,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string faultHandlerName,
            string faultHandlerId,
            string faultHandlerInstanceId,
            string faultHandlerTypeName,
            string message)
        {
            WriteEvent(
                FaultPropagationWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                fault,
                faultMessage,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                faultHandlerName,
                faultHandlerId,
                faultHandlerInstanceId,
                faultHandlerTypeName,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="fault"></param>
        /// <param name="faultMessage"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="faultHandlerName"></param>
        /// <param name="faultHandlerId"></param>
        /// <param name="faultHandlerInstanceId"></param>
        /// <param name="faultHandlerTypeName"></param>
        /// <param name="message"></param>
        [Event(
            FaultPropagationErrorEventId,
            Level = EventLevel.Error,
            Message = "{23}",
            Keywords = Keywords.FaultPropagation)]
        internal void FaultPropagationError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string fault,
            string faultMessage,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string faultHandlerName,
            string faultHandlerId,
            string faultHandlerInstanceId,
            string faultHandlerTypeName,
            string message)
        {
            WriteEvent(
                FaultPropagationErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                fault,
                faultMessage,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                faultHandlerName,
                faultHandlerId,
                faultHandlerInstanceId,
                faultHandlerTypeName,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceVerboseEventId = 160;
        const int WorkflowInstanceInfoEventId = 161;
        const int WorkflowInstanceWarningEventId = 162;
        const int WorkflowInstanceErrorEventId = 163;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstance(IActivityActorInternal actor, WorkflowInstanceRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance))
                {
                    WorkflowInstanceVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance))
                {
                    WorkflowInstanceInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance))
                {
                    WorkflowInstanceWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance))
                {
                    WorkflowInstanceError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{16}",
            Keywords = Keywords.WorkflowInstance)]
        internal void WorkflowInstanceVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string message)
        {
            WriteEvent(
                WorkflowInstanceVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceInfoEventId,
            Level = EventLevel.Informational,
            Message = "{16}",
            Keywords = Keywords.WorkflowInstance)]
        internal void WorkflowInstanceInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string message)
        {
            WriteEvent(
                WorkflowInstanceInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceWarningEventId,
            Level = EventLevel.Warning,
            Message = "{16}",
            Keywords = Keywords.WorkflowInstance)]
        internal void WorkflowInstanceWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string message)
        {
            WriteEvent(
                WorkflowInstanceWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceErrorEventId,
            Level = EventLevel.Error,
            Message = "{16}",
            Keywords = Keywords.WorkflowInstance)]
        internal void WorkflowInstanceError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string message)
        {
            WriteEvent(
                WorkflowInstanceErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceAbortedVerboseEventId = 170;
        const int WorkflowInstanceAbortedInfoEventId = 171;
        const int WorkflowInstanceAbortedWarningEventId = 172;
        const int WorkflowInstanceAbortedErrorEventId = 173;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstanceAborted(IActivityActorInternal actor, WorkflowInstanceAbortedRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted))
                {
                    WorkflowInstanceAbortedVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted))
                {
                    WorkflowInstanceAbortedInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted))
                {
                    WorkflowInstanceAbortedWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted))
                {
                    WorkflowInstanceAbortedError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceAbortedVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted)]
        internal void WorkflowInstanceAbortedVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceAbortedVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceAbortedInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted)]
        internal void WorkflowInstanceAbortedInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceAbortedInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceAbortedWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted)]
        internal void WorkflowInstanceAbortedWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceAbortedWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceAbortedErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceAborted)]
        internal void WorkflowInstanceAbortedError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceAbortedErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceSuspendedVerboseEventId = 180;
        const int WorkflowInstanceSuspendedInfoEventId = 181;
        const int WorkflowInstanceSuspendedWarningEventId = 182;
        const int WorkflowInstanceSuspendedErrorEventId = 183;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstanceSuspended(IActivityActorInternal actor, WorkflowInstanceSuspendedRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended))
                {
                    WorkflowInstanceSuspendedVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended))
                {
                    WorkflowInstanceSuspendedInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended))
                {
                    WorkflowInstanceSuspendedWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended))
                {
                    WorkflowInstanceSuspendedError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceSuspendedVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended)]
        internal void WorkflowInstanceSuspendedVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceSuspendedVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceSuspendedInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended)]
        internal void WorkflowInstanceSuspendedInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceSuspendedInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceSuspendedWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended)]
        internal void WorkflowInstanceSuspendedWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceSuspendedWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceSuspendedErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceSuspended)]
        internal void WorkflowInstanceSuspendedError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceSuspendedErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceTerminatedVerboseEventId = 190;
        const int WorkflowInstanceTerminatedInfoEventId = 191;
        const int WorkflowInstanceTerminatedWarningEventId = 192;
        const int WorkflowInstanceTerminatedErrorEventId = 193;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstanceTerminated(IActivityActorInternal actor, WorkflowInstanceTerminatedRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated))
                {
                    WorkflowInstanceTerminatedVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated))
                {
                    WorkflowInstanceTerminatedInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated))
                {
                    WorkflowInstanceTerminatedWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated))
                {
                    WorkflowInstanceTerminatedError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.Reason) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceTerminatedVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated)]
        internal void WorkflowInstanceTerminatedVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceTerminatedVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceTerminatedInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated)]
        internal void WorkflowInstanceTerminatedInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceTerminatedInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceTerminatedWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated)]
        internal void WorkflowInstanceTerminatedWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceTerminatedWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceTerminatedErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceTerminated)]
        internal void WorkflowInstanceTerminatedError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string reason,
            string message)
        {
            WriteEvent(
                WorkflowInstanceTerminatedErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                reason,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceUnhandledExceptionVerboseEventId = 200;
        const int WorkflowInstanceUnhandledExceptionInfoEventId = 201;
        const int WorkflowInstanceUnhandledExceptionWarningEventId = 202;
        const int WorkflowInstanceUnhandledExceptionErrorEventId = 203;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstanceUnhandledException(IActivityActorInternal actor, WorkflowInstanceUnhandledExceptionRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException))
                {
                    WorkflowInstanceUnhandledExceptionVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.UnhandledException?.Message) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException))
                {
                    WorkflowInstanceUnhandledExceptionInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.UnhandledException?.Message) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException))
                {
                    WorkflowInstanceUnhandledExceptionWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.UnhandledException?.Message) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException))
                {
                    WorkflowInstanceUnhandledExceptionError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        (record.FaultSource?.Name) ?? string.Empty,
                        (record.FaultSource?.Id) ?? string.Empty,
                        (record.FaultSource?.InstanceId) ?? string.Empty,
                        (record.FaultSource?.TypeName) ?? string.Empty,
                        (record.UnhandledException?.Message) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUnhandledExceptionVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{21}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException)]
        internal void WorkflowInstanceUnhandledExceptionVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string exception,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUnhandledExceptionVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                exception,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUnhandledExceptionInfoEventId,
            Level = EventLevel.Informational,
            Message = "{21}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException)]
        internal void WorkflowInstanceUnhandledExceptionInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string exception,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUnhandledExceptionInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                exception,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUnhandledExceptionWarningEventId,
            Level = EventLevel.Warning,
            Message = "{21}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException)]
        internal void WorkflowInstanceUnhandledExceptionWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string exception,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUnhandledExceptionWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                exception,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="faultSourceName"></param>
        /// <param name="faultSourceId"></param>
        /// <param name="faultSourceInstanceId"></param>
        /// <param name="faultSourceTypeName"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUnhandledExceptionErrorEventId,
            Level = EventLevel.Error,
            Message = "{21}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUnhandledException)]
        internal void WorkflowInstanceUnhandledExceptionError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            string faultSourceName,
            string faultSourceId,
            string faultSourceInstanceId,
            string faultSourceTypeName,
            string exception,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUnhandledExceptionErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                faultSourceName,
                faultSourceId,
                faultSourceInstanceId,
                faultSourceTypeName,
                exception,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int WorkflowInstanceUpdatedVerboseEventId = 210;
        const int WorkflowInstanceUpdatedInfoEventId = 211;
        const int WorkflowInstanceUpdatedWarningEventId = 212;
        const int WorkflowInstanceUpdatedErrorEventId = 213;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void WorkflowInstanceUpdated(IActivityActorInternal actor, WorkflowInstanceUpdatedRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated))
                {
                    WorkflowInstanceUpdatedVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        record.IsSuccessful,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated))
                {
                    WorkflowInstanceUpdatedInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        record.IsSuccessful,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated))
                {
                    WorkflowInstanceUpdatedWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        record.IsSuccessful,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated))
                {
                    WorkflowInstanceUpdatedError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.State) ?? string.Empty,
                        (record.WorkflowDefinitionIdentity?.Name) ?? string.Empty,
                        (record.ActivityDefinitionId) ?? string.Empty,
                        record.IsSuccessful,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUpdatedVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated)]
        internal void WorkflowInstanceUpdatedVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            bool isSuccessful,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUpdatedVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                isSuccessful,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUpdatedInfoEventId,
            Level = EventLevel.Informational,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated)]
        internal void WorkflowInstanceUpdatedInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            bool isSuccessful,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUpdatedInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                isSuccessful,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUpdatedWarningEventId,
            Level = EventLevel.Warning,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated)]
        internal void WorkflowInstanceUpdatedWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            bool isSuccessful,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUpdatedWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                isSuccessful,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="state"></param>
        /// <param name="workflowDefinitionIdentityName"></param>
        /// <param name="activityDefinitionId"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        [Event(
            WorkflowInstanceUpdatedErrorEventId,
            Level = EventLevel.Error,
            Message = "{17}",
            Keywords = Keywords.WorkflowInstance | Keywords.WorkflowInstanceUpdated)]
        internal void WorkflowInstanceUpdatedError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string state,
            string workflowDefinitionIdentityName,
            string activityDefinitionId,
            bool isSuccessful,
            string message)
        {
            WriteEvent(
                WorkflowInstanceUpdatedErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                state,
                workflowDefinitionIdentityName,
                activityDefinitionId,
                isSuccessful,
                message);
        }


    }

    sealed partial class ActivityActorEventSource
    {

        const int CustomTrackingVerboseEventId = 300;
        const int CustomTrackingInfoEventId = 301;
        const int CustomTrackingWarningEventId = 302;
        const int CustomTrackingErrorEventId = 303;
        
        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [NonEvent]
        internal void CustomTracking(IActivityActorInternal actor, CustomTrackingRecord record, string message = "", params object[] args)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(record != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentNullException>(args != null);

            if (IsEnabled())
            {
                if (record.Level == TraceLevel.Verbose && IsEnabled(EventLevel.Verbose, Keywords.CustomTracking))
                {
                    CustomTrackingVerbose(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Name) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Data)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Info && IsEnabled(EventLevel.Informational, Keywords.CustomTracking))
                {
                    CustomTrackingInfo(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Name) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Data)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Warning && IsEnabled(EventLevel.Warning, Keywords.CustomTracking))
                {
                    CustomTrackingWarning(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Name) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Data)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

                if (record.Level == TraceLevel.Error && IsEnabled(EventLevel.Error, Keywords.CustomTracking))
                {
                    CustomTrackingError(
                        actor.GetType().ToString(),
                        actor.Id.ToString(),
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                        actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                        actor.ActorService.Context.ServiceTypeName,
                        actor.ActorService.Context.ServiceName.ToString(),
                        actor.ActorService.Context.PartitionId,
                        actor.ActorService.Context.ReplicaId,
                        FabricRuntime.GetNodeContext().NodeName,
                        record.InstanceId,
                        record.RecordNumber,
                        record.EventTime.ToFileTimeUtc(),
                        PrepareAnnotations(record.Annotations),
                        (record.Name) ?? string.Empty,
                        (record.Activity?.Name) ?? string.Empty,
                        (record.Activity?.Id) ?? string.Empty,
                        (record.Activity?.InstanceId) ?? string.Empty,
                        (record.Activity?.TypeName) ?? string.Empty,
                        (PrepareDictionary(record.Data)) ?? string.Empty,
                        string.Format(message ?? string.Empty, args));
                    return;
                }

            }
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="name"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        [Event(
            CustomTrackingVerboseEventId,
            Level = EventLevel.Verbose,
            Message = "{19}",
            Keywords = Keywords.CustomTracking)]
        internal void CustomTrackingVerbose(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string name,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string data,
            string message)
        {
            WriteEvent(
                CustomTrackingVerboseEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                name,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                data,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="name"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        [Event(
            CustomTrackingInfoEventId,
            Level = EventLevel.Informational,
            Message = "{19}",
            Keywords = Keywords.CustomTracking)]
        internal void CustomTrackingInfo(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string name,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string data,
            string message)
        {
            WriteEvent(
                CustomTrackingInfoEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                name,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                data,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="name"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        [Event(
            CustomTrackingWarningEventId,
            Level = EventLevel.Warning,
            Message = "{19}",
            Keywords = Keywords.CustomTracking)]
        internal void CustomTrackingWarning(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string name,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string data,
            string message)
        {
            WriteEvent(
                CustomTrackingWarningEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                name,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                data,
                message);
        }


        /// <summary>
        /// Records an event.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="actorId"></param>
        /// <param name="applicationTypeName"></param>
        /// <param name="applicationName"></param>
        /// <param name="serviceTypeName"></param>
        /// <param name="serviceName"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <param name="nodeName"></param>
        /// <param name="instanceId"></param>
        /// <param name="recordNumber"></param>
        /// <param name="eventTime"></param>
        /// <param name="annotations"></param>
        /// <param name="name"></param>
        /// <param name="activityName"></param>
        /// <param name="activityId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityTypeName"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        [Event(
            CustomTrackingErrorEventId,
            Level = EventLevel.Error,
            Message = "{19}",
            Keywords = Keywords.CustomTracking)]
        internal void CustomTrackingError(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            Guid instanceId,
            long recordNumber,
            long eventTime,
            string annotations,
            string name,
            string activityName,
            string activityId,
            string activityInstanceId,
            string activityTypeName,
            string data,
            string message)
        {
            WriteEvent(
                CustomTrackingErrorEventId,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                instanceId,
                recordNumber,
                eventTime,
                annotations,
                name,
                activityName,
                activityId,
                activityInstanceId,
                activityTypeName,
                data,
                message);
        }


    }
}
