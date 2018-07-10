using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Represents a <see cref="Actor"/> that hosts a Windows Workflow Foundation activity.
    /// </summary>
    /// <remarks>
    /// This base contains the main implementation details.
    /// </remarks>
    public abstract class ActivityActorCore :
        Cogito.ServiceFabric.Actors.Actor,
        IRemindable,
        IActivityActorInternal,
        IActivityActor
    {

        readonly ActivityWorkflowHost host;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ActivityActorCore(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
            host = new ActivityWorkflowHost(this);
        }

        /// <summary>
        /// Gets a reference to the workflow host.
        /// </summary>
        internal ActivityWorkflowHost Host
        {
            get { return host; }
        }

        /// <summary>
        /// Overrides the <see cref="OnActivateAsync"/> method so it can be reimplemented above.
        /// </summary>
        /// <returns></returns>
        protected sealed override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            await OnActivateAsyncInternal();
        }

        /// <summary>
        /// New implementation of <see cref="OnActivateAsync"/>.
        /// </summary>
        /// <returns></returns>
        async Task OnActivateAsyncInternal()
        {
            await OnBeforeWorkflowActivateAsync();
            await host.OnActivateAsync();
            await OnAfterWorkflowActivateAsync();
            await OnActivateAsyncHidden();
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers. This method is called
        /// right after the actor is activated and before any method call or reminders are dispatched on it.
        /// </summary>
        /// <returns></returns>
        protected internal abstract Task OnActivateAsyncHidden();

        /// <summary>
        /// Overrides the <see cref="OnDeactivateAsync"/> method so it can be reimplemented above.
        /// </summary>
        /// <returns></returns>
        protected sealed override async Task OnDeactivateAsync()
        {
            await base.OnDeactivateAsync();
            await OnDeactivateAsyncInternal();
        }

        /// <summary>
        /// New implementation of <see cref="OnDeactivateAsync"/>.
        /// </summary>
        /// <returns></returns>
        async Task OnDeactivateAsyncInternal()
        {
            await OnBeforeWorkflowDeactivateAsync();
            await host.OnDeactivateAsync();
            await OnAfterWorkflowDeactivateAsync();
            await OnDeactivateAsyncHidden();
        }

        /// <summary>
        ///  Override this method to release any resources including unregistering the timers. This method is called
        ///  right before the actor is deactivated.
        /// </summary>
        /// <returns></returns>
        protected internal abstract Task OnDeactivateAsyncHidden();

        /// <summary>
        /// Creates a new instance of <see cref="Activity"/>. Override this method to customize the instance.
        /// </summary>
        /// <returns></returns>
        protected abstract Activity CreateActivity();

        /// <summary>
        /// Creates a new instance of <see cref="Activity"/>.
        /// </summary>
        /// <returns></returns>
        internal Activity CreateActivityInternal()
        {
            return CreateActivity();
        }

        /// <summary>
        /// Creates the set of activity parameters to be passed to the workflow.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, object> CreateActivityInputs()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the set of custom extensions to add to the workflow.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<object> GetWorkflowExtensions()
        {
            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Invoked before the workflow has been activated.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnBeforeWorkflowActivateAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked after the workflow has been activated.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnAfterWorkflowActivateAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked before the workflow has been deactivated.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnBeforeWorkflowDeactivateAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked after the workfly has been deactivated.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnAfterWorkflowDeactivateAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Exception"/> is thrown during the workflow operation.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnUnhandledExceptionAsync(WorkflowApplicationUnhandledExceptionEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the <see cref="ActivityActor"/> is aborted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnAbortedAsync(WorkflowApplicationAbortedEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the <see cref="ActivityActor"/> is idle and persistable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnPersistableIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the <see cref="ActivityActor"/> goes idle.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the <see cref="ActivityActor"/> is completed.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnCompletedAsync(WorkflowApplicationCompletedEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the <see cref="ActivityActor"/> is unloaded.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual Task OnUnloadedAsync(WorkflowApplicationEventArgs args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the activity workflow is persisted to the <see cref="IActorStateManager"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnPersistedAsync()
        {
            return SaveStateAsync();
        }

        /// <summary>
        /// Resets the workflow.
        /// </summary>
        /// <returns></returns>
        protected Task ResetAsync()
        {
            return host.ResetAsync();
        }

        /// <summary>
        /// Runs the workflow.
        /// </summary>
        /// <returns></returns>
        protected Task RunAsync()
        {
            return host.RunAsync();
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName, object value, TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            return host.ResumeAsync(bookmarkName, value, timeout);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName, object value)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            return host.ResumeAsync(bookmarkName, value);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName, TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            return host.ResumeAsync(bookmarkName, null, timeout);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/>.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(string bookmarkName)
        {
            if (string.IsNullOrEmpty(bookmarkName))
                throw new ArgumentOutOfRangeException(nameof(bookmarkName));

            return host.ResumeAsync(bookmarkName, null);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/>.
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(Bookmark bookmark, TimeSpan timeout)
        {
            if (bookmark == null)
                throw new ArgumentNullException(nameof(bookmark));

            return host.ResumeAsync(bookmark, null, timeout);
        }

        /// <summary>
        /// Resumes the <see cref="ActivityActor{TState}"/>.
        /// </summary>
        /// <param name="bookmark"></param>
        /// <returns></returns>
        /// <exception cref="ActivityStateException"></exception>
        protected Task<BookmarkResumptionResult> ResumeAsync(Bookmark bookmark)
        {
            if (bookmark == null)
                throw new ArgumentNullException(nameof(bookmark));

            return host.ResumeAsync(bookmark, null);
        }

        /// <summary>
        /// Implements the <see cref="IRemindable.ReceiveReminderAsync(string, byte[], TimeSpan, TimeSpan)"/> method so it can be reimplemented above.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="context"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        Task IRemindable.ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            return ReceiveReminderAsyncInternal(reminderName, context, dueTime, period);
        }

        /// <summary>
        /// Invoked when a reminder is fired.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="context"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        async Task ReceiveReminderAsyncInternal(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            await host.ReceiveReminderAsync(reminderName, context, dueTime, period);
            await ReceiveReminderAsync(reminderName, context, dueTime, period);
        }

        /// <summary>
        /// Reminder call back invoked when an actor reminder is triggered.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="context"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        protected virtual Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            return Task.FromResult(true);
        }

        #region IActivityActorInternal

        Activity IActivityActorInternal.CreateActivity()
        {
            return CreateActivity();
        }

        IDictionary<string, object> IActivityActorInternal.CreateActivityInputs()
        {
            return CreateActivityInputs() ?? new Dictionary<string, object>();
        }

        IEnumerable<object> IActivityActorInternal.GetWorkflowExtensions()
        {
            return GetWorkflowExtensions();
        }

        IActorStateManager IActivityActorInternal.StateManager
        {
            get { return StateManager; }
        }

        IActorTimer IActivityActorInternal.RegisterTimer(Func<object, Task> callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            return RegisterTimer(callback, state, dueTime, period);
        }

        void IActivityActorInternal.UnregisterTimer(IActorTimer timer)
        {
            UnregisterTimer(timer);
        }

        void IActivityActorInternal.InvokeWithTimer(Func<Task> action)
        {
            InvokeWithTimer(action);
        }

        Task IActivityActorInternal.InvokeWithTimerAsync(Func<Task> action)
        {
            return InvokeWithTimerAsync(action);
        }

        Task<TResult> IActivityActorInternal.InvokeWithTimerAsync<TResult>(Func<Task<TResult>> func)
        {
            return InvokeWithTimerAsync(func);
        }

        IActorReminder IActivityActorInternal.GetReminder(string reminderName)
        {
            return GetReminder(reminderName);
        }

        IActorReminder IActivityActorInternal.TryGetReminder(string reminderName)
        {
            return TryGetReminder(reminderName);
        }

        Task<IActorReminder> IActivityActorInternal.RegisterReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            return RegisterReminderAsync(reminderName, state, dueTime, period);
        }

        Task IActivityActorInternal.UnregisterReminderAsync(IActorReminder reminder)
        {
            return UnregisterReminderAsync(reminder);
        }

        async Task IActivityActorInternal.OnPersistedAsync()
        {
            await OnPersistedAsync();
        }

        Task IActivityActorInternal.OnUnhandledExceptionAsync(WorkflowApplicationUnhandledExceptionEventArgs args)
        {
            return OnUnhandledExceptionAsync(args);
        }

        Task IActivityActorInternal.OnAbortedAsync(WorkflowApplicationAbortedEventArgs args)
        {
            return OnAbortedAsync(args);
        }

        Task IActivityActorInternal.OnPersistableIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            return OnPersistableIdleAsync(args);
        }

        Task IActivityActorInternal.OnIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            return OnIdleAsync(args);
        }

        Task IActivityActorInternal.OnCompletedAsync(WorkflowApplicationCompletedEventArgs args)
        {
            return OnCompletedAsync(args);
        }

        Task IActivityActorInternal.OnUnloadedAsync(WorkflowApplicationEventArgs args)
        {
            return OnUnloadedAsync(args);
        }

        Task IActivityActor.ResumeAsync(string bookmarkName, object value)
        {
            return ResumeAsync(bookmarkName, value);
        }

        #endregion

    }

}
