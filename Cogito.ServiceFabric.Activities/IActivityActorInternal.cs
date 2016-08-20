using System;
using System.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Provides access to some methods of a <see cref="Actor"/> that can host a <see cref="Activity"/>.
    /// </summary>
    interface IActivityActorInternal
    {

        /// <summary>
        /// Gets the ID of the actor.
        /// </summary>
        ActorId Id { get; }

        /// <summary>
        /// Gets the <see cref="ActorService"/>.
        /// </summary>
        ActorService ActorService { get; }

        /// <summary>
        /// Gets a reference to the <see cref="IActorStateManager"/>.
        /// </summary>
        IActorStateManager StateManager { get; }

        /// <summary>
        /// Creates the <see cref="Activity"/> associated with the actor.
        /// </summary>
        /// <returns></returns>
        Activity CreateActivity();

        /// <summary>
        /// Creates the set of parameters to be passed to the workflow.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> CreateActivityInputs();

        /// <summary>
        /// Gets the set of custom extensions to add to the workflow.
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetWorkflowExtensions();

        /// <summary>
        /// Registers a timer with the actor.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        IActorTimer RegisterTimer(Func<object, Task> callback, object state, TimeSpan dueTime, TimeSpan period);

        /// <summary>
        /// Unregisters a timer with the actor.
        /// </summary>
        /// <param name="timer"></param>
        void UnregisterTimer(IActorTimer timer);

        /// <summary>
        /// Invokes the given action once on an actor timer.
        /// </summary>
        /// <param name="action"></param>
        void InvokeWithTimer(Func<Task> action);

        /// <summary>
        /// Invokes the given action once on an actor timer.
        /// </summary>
        /// <param name="action"></param>
        Task InvokeWithTimerAsync(Func<Task> action);

        /// <summary>
        /// Invokes the given action once on an actor timer.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        Task<TResult> InvokeWithTimerAsync<TResult>(Func<Task<TResult>> func);

        /// <summary>
        /// Gets the reminder with the specified name.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <returns></returns>
        IActorReminder GetReminder(string reminderName);

        /// <summary>
        /// Tries to get the reminder with the specified name, or returns null.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <returns></returns>
        IActorReminder TryGetReminder(string reminderName);

        /// <summary>
        /// Registers the specified reminder.
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="state"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        Task<IActorReminder> RegisterReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period);

        /// <summary>
        ///  Unregisters the specified reminder.
        /// </summary>
        /// <param name="reminder"></param>
        /// <returns></returns>
        Task UnregisterReminderAsync(IActorReminder reminder);

        /// <summary>
        /// Invoked when the activity workflow is persisted to the <see cref="IActorStateManager"/>.
        /// </summary>
        /// <returns></returns>
        Task OnPersistedAsync();

        /// <summary>
        /// Invoked when an unhandled <see cref="Exception"/> occurs.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnUnhandledExceptionAsync(WorkflowApplicationUnhandledExceptionEventArgs args);

        /// <summary>
        /// Invoked when the workflow is aborted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnAbortedAsync(WorkflowApplicationAbortedEventArgs args);

        /// <summary>
        /// Invoked when the workflow goes idle.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnIdleAsync(WorkflowApplicationIdleEventArgs args);

        /// <summary>
        /// Invoked when the workflow goes idle and is persitable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnPersistableIdleAsync(WorkflowApplicationIdleEventArgs args);

        /// <summary>
        /// Invoked when the workflow is completed.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnCompletedAsync(WorkflowApplicationCompletedEventArgs args);

        /// <summary>
        /// Invoked when the workflow is unloaded.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnUnloadedAsync(WorkflowApplicationEventArgs args);

    }

}

