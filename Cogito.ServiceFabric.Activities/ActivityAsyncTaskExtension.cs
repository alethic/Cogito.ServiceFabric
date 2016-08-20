using System;
using System.Diagnostics.Contracts;

using Cogito.Activities;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// <see cref="AsyncTaskExtension"/> implementation that dispatches task execution to the actor timer framework.
    /// </summary>
    class ActivityAsyncTaskExtension :
        AsyncTaskExtension
    {

        readonly ActivityWorkflowHost host;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public ActivityAsyncTaskExtension(ActivityWorkflowHost host)
            : base(new ActivityAsyncTaskExecutor(host))
        {
            Contract.Requires<ArgumentNullException>(host != null);
        }

    }

}
