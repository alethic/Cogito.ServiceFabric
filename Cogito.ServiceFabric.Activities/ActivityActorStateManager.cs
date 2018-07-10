using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Manages access to a workflow's state with a <see cref="IActorStateManager"/>.
    /// </summary>
    class ActivityActorStateManager
    {

        const string STATE_NAME = "__ActivityActorState__";

        readonly ActivityWorkflowHost host;

        // instance state
        Guid instanceOwnerId;
        Guid instanceId;
        InstanceState instanceState;
        ImmutableDictionary<XName, object> instanceData = ImmutableDictionary<XName, object>.Empty;
        ImmutableDictionary<XName, object> instanceMetadata = ImmutableDictionary<XName, object>.Empty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public ActivityActorStateManager(ActivityWorkflowHost host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        /// <summary>
        /// Gets or sets the instance owner id.
        /// </summary>
        public Guid InstanceOwnerId
        {
            get { return instanceOwnerId; }
            set { instanceOwnerId = value; }
        }

        /// <summary>
        /// Gets or sets the instance id.
        /// </summary>
        public Guid InstanceId
        {
            get { return instanceId; }
            set { instanceId = value; }
        }

        /// <summary>
        /// Gets or sets the instance state.
        /// </summary>
        public InstanceState InstanceState
        {
            get { return instanceState; }
            set { instanceState = value; }
        }

        /// <summary>
        /// Sets an instance data item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInstanceData(XName name, object value)
        {
            instanceData = instanceData.SetItem(name, value);
        }

        /// <summary>
        /// Gets an instance data item.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetInstanceData(XName name)
        {
            return instanceData.GetValueOrDefault(name);
        }

        /// <summary>
        /// Gets the available instance data names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XName> GetInstanceDataNames()
        {
            return instanceData.Keys;
        }

        /// <summary>
        /// Clears the instance metadata.
        /// </summary>
        public void ClearInstanceData()
        {
            instanceData = instanceData.Clear();
        }

        /// <summary>
        /// Sets an instance metadata item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInstanceMetadata(XName name, object value)
        {
            instanceMetadata = instanceMetadata.SetItem(name, value);
        }

        /// <summary>
        /// Gets an instance metadata item.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetInstanceMetadata(XName name)
        {
            return instanceMetadata.GetValueOrDefault(name);
        }

        /// <summary>
        /// Gets the available instance data names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XName> GetInstanceMetadataNames()
        {
            return instanceMetadata.Keys;
        }

        /// <summary>
        /// Clears the instance metadata.
        /// </summary>
        public void ClearInstanceMetadata()
        {
            instanceMetadata = instanceMetadata.Clear();
        }

        /// <summary> 
        /// Loads the state from the actor. Should be invoked from a context with an actor lock.
        /// </summary>
        /// <returns></returns>
        public async Task LoadAsync()
        {
            var state = await host.Actor.StateManager.TryGetStateAsync<ActivityActorState>(STATE_NAME);
            if (state.HasValue)
            {
                instanceOwnerId = state.Value.InstanceOwnerId;
                instanceId = state.Value.InstanceId;
                instanceState = state.Value.InstanceState;
                instanceData = state.Value.InstanceData.ToImmutableDictionary();
                instanceMetadata = state.Value.InstanceMetadata.ToImmutableDictionary();
            }
            else
            {
                instanceOwnerId = Guid.Empty;
                instanceId = Guid.Empty;
                instanceState = InstanceState.Unknown;
                instanceData = ImmutableDictionary<XName, object>.Empty;
                instanceMetadata = ImmutableDictionary<XName, object>.Empty;
            }
        }

        /// <summary>
        /// Saves the state to the actor. Should be invoked from a context with an actor lock.
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            await host.Actor.StateManager.SetStateAsync(STATE_NAME, new ActivityActorState()
            {
                InstanceOwnerId = instanceOwnerId,
                InstanceId = instanceId,
                InstanceState = instanceState,
                InstanceData = instanceData.ToArray(),
                InstanceMetadata = instanceMetadata.ToArray(),
            });
        }

        /// <summary>
        /// Raised by the instance store after persistence.
        /// </summary>
        public Action Persisted;

        /// <summary>
        /// Raises the Persisted event.
        /// </summary>
        public void OnPersisted()
        {
            Persisted?.Invoke();
        }

        /// <summary>
        /// Raised by the instance store upon completion.
        /// </summary>
        public Action Completed;

        /// <summary>
        /// Raises the Completed event.
        /// </summary>
        /// <returns></returns>
        public void OnCompleted()
        {
            Completed?.Invoke();
        }

    }

}
