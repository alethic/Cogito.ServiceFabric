using System;
using System.Activities.Tracking;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Cogito.Threading;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Dispatches workflow events to the <see cref="ActivityActorEventSource"/>.
    /// </summary>
    partial class ActivityActorTrackingParticipant :
        TrackingParticipant
    {

        readonly IActivityActorInternal actor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ActivityActorTrackingParticipant(IActivityActorInternal actor)
        {
            Contract.Requires<ArgumentNullException>(actor != null);

            this.actor = actor;
        }

        /// <summary>
        /// Begins tracking a record.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="timeout"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginTrack(TrackingRecord record, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Task.Run(() => Track(record, timeout)).ToAsyncBegin(callback, state);
        }

        /// <summary>
        /// Ends tracking a record
        /// </summary>
        /// <param name="result"></param>
        protected override void EndTrack(IAsyncResult result)
        {
            ((Task)result).ToAsyncEnd();
        }

        /// <summary>
        /// Tracks a record.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="timeout"></param>
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            if (ActivityActorEventSource.Current.IsEnabled())
            {
                TrackRecord(record);
            }
        }

    }

}
