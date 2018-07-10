using System;

using Cogito.Activities;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// <see cref="AsyncTaskExtension"/> implementation that dispatches task execution to the actor timer framework.
    /// </summary>
    class ActivityAsyncTaskExtension :
        AsyncTaskExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public ActivityAsyncTaskExtension(ActivityWorkflowHost host)
            : base(new ActivityAsyncTaskExecutor(host))
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
        }

    }

}
