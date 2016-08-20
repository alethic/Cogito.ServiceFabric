using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using Cogito.Activities;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// <see cref="AsyncTaskExecutor"/> implementation that submits to the <see cref="ActivityWorkflowHost"/>.
    /// </summary>
    class ActivityAsyncTaskExecutor :
        AsyncTaskExecutor
    {

        readonly ActivityWorkflowHost host;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public ActivityAsyncTaskExecutor(ActivityWorkflowHost host) 
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

        /// <summary>
        /// Submits the action to the host's task pump.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override Task ExecuteAsync(Func<Task> action)
        {
            return host.Pump.Enqueue(action);
        }

        /// <summary>
        /// Submits the action to the host's task pump.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public override Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func)
        {
            return host.Pump.Enqueue(func);
        }

    }

}
