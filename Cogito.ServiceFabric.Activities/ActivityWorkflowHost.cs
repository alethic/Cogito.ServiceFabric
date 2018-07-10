using System;
using System.Activities;
using System.Collections.Generic;
using System.Runtime.DurableInstancing;
using System.Threading.Tasks;
using System.Xml.Linq;

using Cogito.Activities;
using Cogito.Threading;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Manages the <see cref="WorkflowApplication"/> associated with a <see cref="Activity"/> actor.
    /// </summary>
    class ActivityWorkflowHost
    {

        static readonly XNamespace WorkflowNamespace = "urn:schemas-microsoft-com:System.Activities/4.0/properties";
        static readonly XName ActivityTimerExpirationTimeKey = WorkflowNamespace + "TimerExpirationTime";
        static readonly string ActivityTimerExpirationReminderName = "Cogito.ServiceFabric.Activities::TimerExpirationReminder";

        readonly IActivityActorInternal actor;
        readonly TaskPump pump;
        ActivityActorStateManager state;
        WorkflowApplication workflow;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="actor"></param>
        public ActivityWorkflowHost(IActivityActorInternal actor)
        {
            this.actor = actor ?? throw new ArgumentNullException(nameof(actor));

            // to enqueue task functions to execute in actor context
            pump = new TaskPump();
            pump.TaskAdded += (s, a) => actor.InvokeWithTimer(pump.PumpOneAsync);
        }

        /// <summary>
        /// Gets a reference to the <see cref="TaskPump"/> used for queuing work items on the actor context.
        /// </summary>
        internal TaskPump Pump
        {
            get { return pump; }
        }

        /// <summary>
        /// Invokes the specified method and then pumps any tasks in the queue.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        async Task InvokeAndPump(Func<Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var t = action();
            await pump.PumpAsync();
            await t;
        }

        /// <summary>
        /// Invokes the specified method and then pumps any tasks in the queue. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        async Task<TResult> InvokeAndPump<TResult>(Func<Task<TResult>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var t = func();
            await pump.PumpAsync();
            return await t;
        }

        /// <summary>
        /// Gets the associated actor.
        /// </summary>
        internal IActivityActorInternal Actor
        {
            get { return actor; }
        }

        /// <summary>
        /// Gets the running workflow application.
        /// </summary>
        internal WorkflowApplication Workflow
        {
            get { return workflow; }
        }

        /// <summary>
        /// Throws if the workflow is in an invalid state.
        /// </summary>
        void ThrowIfInvalidState()
        {
            if (workflow == null)
                throw new ActivityStateException();
            if (state.InstanceId == Guid.Empty)
                throw new ActivityStateException();
            if (state.InstanceState == InstanceState.Unknown ||
                state.InstanceState == InstanceState.Completed)
                throw new ActivityStateException();
        }

        /// <summary>
        /// Creates a new <see cref="ActivityActorStateManager"/>.
        /// </summary>
        Task CreateStateManager()
        {
            state = new ActivityActorStateManager(this);

            state.Persisted = () =>
            {
                pump.Enqueue(async () =>
                {
                    await state.SaveAsync();
                    await SaveReminderAsync();
                    await actor.OnPersistedAsync();
                });
            };

            state.Completed = () =>
            {
                pump.Enqueue(async () =>
                {
                    await state.SaveAsync();
                    await SaveReminderAsync();
                });
            };

            return Task.FromResult(true);
        }

        /// <summary>
        /// Create a new <see cref="WorkflowApplication"/> instance.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task CreateWorkflow(Activity activity, IDictionary<string, object> inputs = null)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            // manages access to the workflow state
            workflow = inputs != null ? new WorkflowApplication(activity, inputs) : new WorkflowApplication(activity);
            workflow.InstanceStore = new ActivityActorInstanceStore(state);

            workflow.OnUnhandledException = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnUnhandledExceptionAsync(args);
                });

                return UnhandledExceptionAction.Abort;
            };

            workflow.Aborted = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnAbortedAsync(args);
                });
            };

            workflow.PersistableIdle = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnPersistableIdleAsync(args);
                });

                // workflow should save state but not unload until actor deactivated
                // workflow timers invoke themselves as long as it's loaded
                return PersistableIdleAction.Persist;
            };

            workflow.Idle = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnIdleAsync(args);
                });
            };

            workflow.Completed = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnCompletedAsync(args);
                });
            };

            workflow.Unloaded = args =>
            {
                pump.Enqueue(async () =>
                {
                    await actor.OnUnloadedAsync(args);
                });
            };

            workflow.Extensions.Add(() => new ActivityAsyncTaskExtension(this));
            workflow.Extensions.Add(() => new ActivityActorTrackingParticipant(actor));
            workflow.Extensions.Add(() => new ActivityActorExtension(actor));

            // add user extensions
            foreach (var ext in actor.GetWorkflowExtensions())
                workflow.Extensions.Add(ext);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Attempts to load the workflow if possible.
        /// </summary>
        /// <returns></returns>
        async Task LoadWorkflow()
        {
            // might already be loaded somehow
            if (workflow == null)
            {
                // load state from actor
                await state.LoadAsync();

                // only continue if not already completed
                if (state.InstanceState != InstanceState.Completed)
                {
                    // generate owner ID
                    if (state.InstanceOwnerId == Guid.Empty)
                        state.InstanceOwnerId = Guid.NewGuid();

                    if (state.InstanceId == Guid.Empty)
                    {
                        // clear existing data
                        state.ClearInstanceData();
                        state.ClearInstanceMetadata();

                        // create workflow application
                        await CreateWorkflow(actor.CreateActivity(), actor.CreateActivityInputs() ?? new Dictionary<string, object>());
                        state.InstanceId = workflow.Id;
                        await InvokeAndPump(workflow.RunAsync);
                        await state.SaveAsync();
                    }
                    else
                    {
                        // create workflow application
                        await CreateWorkflow(actor.CreateActivity());
                        await InvokeAndPump(async () => await workflow.LoadAsync(state.InstanceId));
                        await InvokeAndPump(workflow.RunAsync);
                        await state.SaveAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to unload the workflow if possible.
        /// </summary>
        /// <returns></returns>
        async Task UnloadWorkflow()
        {
            if (workflow != null)
            {
                try
                {
                    await InvokeAndPump(workflow.UnloadAsync);
                }
                catch (TimeoutException)
                {

                }
                finally
                {
                    workflow = null;
                }
            }
        }

        /// <summary>
        /// Invoked when the actor is activated.
        /// </summary>
        /// <returns></returns>
        internal async Task OnActivateAsync()
        {
            await CreateStateManager();
            await LoadWorkflow();
        }

        /// <summary>
        /// Invoked when the actor is deactiviated.
        /// </summary>
        /// <returns></returns>
        internal async Task OnDeactivateAsync()
        {
            await UnloadWorkflow();
        }

        /// <summary>
        /// Ensures workflow timers are registered for waking the instance up.
        /// </summary>
        /// <returns></returns>
        async Task SaveReminderAsync()
        {
            // next date at which the reminder should be invoked
            var dueDate = (DateTime?)state.GetInstanceData(ActivityTimerExpirationTimeKey) ?? DateTime.MinValue;
            if (dueDate != DateTime.MinValue)
            {
                // lock down current time
                var nowDate = DateTime.UtcNow;

                // don't allow dates in the past
                if (dueDate < nowDate)
                    dueDate = nowDate;

                // unregister reminder if it the time has changed
                var reminder = actor.TryGetReminder(ActivityTimerExpirationReminderName);
                if (reminder != null)
                {
                    // deserialize last due date from reminder
                    var lastDueDate = DateTime.FromBinary(BitConverter.ToInt64(reminder.State, 0));
                    if (lastDueDate != dueDate)
                    {
                        // timer is out of range, will reschedule below
                        await actor.UnregisterReminderAsync(reminder);
                        reminder = null;
                    }
                }

                // schedule new reminder
                if (reminder == null)
                    await actor.RegisterReminderAsync(
                        ActivityTimerExpirationReminderName,
                        BitConverter.GetBytes(dueDate.ToBinary()),
                        (dueDate - nowDate).Max(TimeSpan.FromMilliseconds(100)),
                        TimeSpan.FromMilliseconds(-1));
            }
            else
            {
                // no reminder required, unregister existing reminder
                var reminder = actor.TryGetReminder(ActivityTimerExpirationReminderName);
                if (reminder != null)
                    await actor.UnregisterReminderAsync(reminder);
            }
        }

        /// <summary>
        /// Resets the workflow.
        /// </summary>
        /// <returns></returns>
        internal async Task ResetAsync()
        {
            await UnloadWorkflow();
            state.InstanceId = Guid.Empty;
            state.InstanceState = InstanceState.Unknown;
            state.ClearInstanceData();
            state.ClearInstanceMetadata();
            await state.SaveAsync();
            await LoadWorkflow();
        }

        /// <summary>
        /// Runs the workflow.
        /// </summary>
        /// <returns></returns>
        internal async Task RunAsync()
        {
            ThrowIfInvalidState();
            await InvokeAndPump(workflow.RunAsync);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TActivity}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal async Task<BookmarkResumptionResult> ResumeAsync(Bookmark bookmark, object value, TimeSpan timeout)
        {
            if (bookmark == null)
                throw new ArgumentNullException(nameof(bookmark));

            ThrowIfInvalidState();

            return await InvokeAndPump(() => workflow.ResumeBookmarkAsync(bookmark, value, timeout));
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TActivity}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal async Task<BookmarkResumptionResult> ResumeAsync(Bookmark bookmark, object value)
        {
            if (bookmark == null)
                throw new ArgumentNullException(nameof(bookmark));

            ThrowIfInvalidState();

            return await InvokeAndPump(() => workflow.ResumeBookmarkAsync(bookmark, value));
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TActivity}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal async Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName, object value, TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            ThrowIfInvalidState();

            return await InvokeAndPump(() => workflow.ResumeBookmarkAsync(bookmarkName, value, timeout));
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TActivity}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal async Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName, object value)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            ThrowIfInvalidState();

            return await InvokeAndPump(() => workflow.ResumeBookmarkAsync(bookmarkName, value));
        }

        /// <summary>
        /// Invoked when a reminder is fired.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="context"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        internal Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName == ActivityTimerExpirationReminderName)
            {
                // workflow was activated, should be good enough
            }

            return Task.FromResult(true);
        }

    }

}
