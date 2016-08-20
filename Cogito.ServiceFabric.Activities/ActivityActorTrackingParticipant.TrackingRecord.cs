using System.Activities.Tracking;

namespace Cogito.ServiceFabric.Activities
{

	public partial class ActivityActorTrackingParticipant
	{

		void TrackRecord(TrackingRecord record)
		{
			if (record is CustomTrackingRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.CustomTracking((IActivityActorInternal)actor, (CustomTrackingRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceUpdatedRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstanceUpdated((IActivityActorInternal)actor, (WorkflowInstanceUpdatedRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceUnhandledExceptionRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstanceUnhandledException((IActivityActorInternal)actor, (WorkflowInstanceUnhandledExceptionRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceTerminatedRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstanceTerminated((IActivityActorInternal)actor, (WorkflowInstanceTerminatedRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceSuspendedRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstanceSuspended((IActivityActorInternal)actor, (WorkflowInstanceSuspendedRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceAbortedRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstanceAborted((IActivityActorInternal)actor, (WorkflowInstanceAbortedRecord)record);
					return;
				}
			}

			if (record is WorkflowInstanceRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.WorkflowInstance((IActivityActorInternal)actor, (WorkflowInstanceRecord)record);
					return;
				}
			}

			if (record is FaultPropagationRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.FaultPropagation((IActivityActorInternal)actor, (FaultPropagationRecord)record);
					return;
				}
			}

			if (record is CancelRequestedRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.CancelRequested((IActivityActorInternal)actor, (CancelRequestedRecord)record);
					return;
				}
			}

			if (record is BookmarkResumptionRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.BookmarkResumption((IActivityActorInternal)actor, (BookmarkResumptionRecord)record);
					return;
				}
			}

			if (record is ActivityStateRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.ActivityState((IActivityActorInternal)actor, (ActivityStateRecord)record);
					return;
				}
			}

			if (record is ActivityScheduledRecord)
			{
				if (actor is IActivityActorInternal)
				{
					ActivityActorEventSource.Current.ActivityScheduled((IActivityActorInternal)actor, (ActivityScheduledRecord)record);
					return;
				}
			}

		}

	}

}