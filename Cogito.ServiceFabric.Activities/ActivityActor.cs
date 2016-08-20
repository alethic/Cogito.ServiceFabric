using System;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors.Runtime;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Represents a <see cref="Actor"/> that hosts a Windows Workflow Foundation activity.
    /// </summary>
    public abstract class ActivityActor :
        ActivityActorBase
    {



    }

    /// <summary>
    /// Represents a <see cref="Actor"/> that hosts a Windows Workflow Foundation activity and has a local state object.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public abstract class ActivityActor<TState> :
        ActivityActor
    {

        const string DEFAULT_STATE_KEY = "__ActorState__";

        /// <summary>
        /// Gets or sets the state name in which the state object is stored.
        /// </summary>
        protected virtual string StateObjectKey { get; set; } = DEFAULT_STATE_KEY;

        /// <summary>
        /// Gets the actor state object.
        /// </summary>
        protected virtual TState State { get; set; }

        /// <summary>
        /// Creates the default state object.
        /// </summary>
        /// <returns></returns>
        protected virtual Task<TState> CreateDefaultStateAsync()
        {
            return Task.FromResult(Activator.CreateInstance<TState>());
        }

        /// <summary>
        /// Override this method to initialize the members.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnBeforeWorkflowActivateAsync()
        {
            await base.OnBeforeWorkflowActivateAsync();
            await LoadStateObjectAsync();
        }

        /// <summary>
        /// Loads the state object. This method is invoked as part of the actor activation.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadStateObjectAsync()
        {
            var o = await StateManager.TryGetStateAsync<TState>(StateObjectKey);
            State = o.HasValue ? o.Value : await CreateDefaultStateAsync();
        }

        /// <summary>
        /// Saves the state object. Invoke this after modifications to the state.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SaveStateObjectAsync()
        {
            if (typeof(TState).IsValueType)
                await StateManager.SetStateAsync(StateObjectKey, State);
            else if (State != null)
                await StateManager.SetStateAsync(StateObjectKey, State);
            else if (await StateManager.ContainsStateAsync(StateObjectKey))
                await StateManager.TryRemoveStateAsync(StateObjectKey);
        }

        /// <summary>
        /// Invoked when the activity workflow is persisted to the <see cref="IActorStateManager"/>.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnPersistedAsync()
        {
            await SaveStateObjectAsync();
            await base.OnPersistedAsync();
        }

    }

}
