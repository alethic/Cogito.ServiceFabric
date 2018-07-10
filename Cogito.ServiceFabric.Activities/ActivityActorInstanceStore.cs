using System;
using System.Activities.DurableInstancing;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Cogito.ServiceFabric.Activities.Serialization;
using Cogito.Threading;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Provides a Durable Instancing store for saving objects into a <see cref="ActivityActorStateManager"/>.
    /// </summary>
    class ActivityActorInstanceStore :
        InstanceStore
    {

        readonly ActivityActorStateManager state;
        readonly NetDataContractSerializer serializer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="state"></param>
        public ActivityActorInstanceStore(ActivityActorStateManager state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.serializer = new NetDataContractSerializer(new StreamingContext(StreamingContextStates.All), int.MaxValue, false, FormatterAssemblyStyle.Full, new ActivitySurrogateSelector());
        }

        /// <summary>
        /// Attempts to execute the given command against the <see cref="InstanceStore"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override bool TryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout)
        {
            return EndTryCommand(BeginTryCommand(context, command, timeout, null, null));
        }

        /// <summary>
        /// Begins an attempt to execute the given command against the <see cref="InstanceStore"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="timeout"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginTryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return CommandAsync(context, command).ToAsyncBegin(callback, state);
        }

        /// <summary>
        /// Ends an attempt to execute the given <see cref="InstancePersistenceCommand"/> against the <see cref="InstanceStore"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool EndTryCommand(IAsyncResult result)
        {
            return ((Task<bool>)result).ToAsyncEnd();
        }

        #region Commands

        /// <summary>
        /// Handles a <see cref="InstancePersistenceCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<bool> CommandAsync(InstancePersistenceContext context, InstancePersistenceCommand command)
        {
            if (command is CreateWorkflowOwnerCommand)
            {
                return CreateWorkflowOwnerCommand(context, (CreateWorkflowOwnerCommand)command);
            }
            else if (command is QueryActivatableWorkflowsCommand)
            {
                return QueryActivatableWorkflowsCommand(context, (QueryActivatableWorkflowsCommand)command);
            }
            else if (command is SaveWorkflowCommand)
            {
                return SaveWorkflowCommand(context, (SaveWorkflowCommand)command);
            }
            else if (command is LoadWorkflowCommand)
            {
                return LoadWorkflowCommand(context, (LoadWorkflowCommand)command);
            }
            else if (command is TryLoadRunnableWorkflowCommand)
            {
                return TryLoadRunnableWorkflowCommand(context, (TryLoadRunnableWorkflowCommand)command);
            }
            else if (command is DeleteWorkflowOwnerCommand)
            {
                return DeleteWorkflowOwnerCommand(context, (DeleteWorkflowOwnerCommand)command);
            }
            else
            {
                return Task.FromResult(true);
            }
        }

        /// <summary>
        /// Handles a <see cref="CreateWorkflowOwnerCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        Task<bool> CreateWorkflowOwnerCommand(InstancePersistenceContext context, CreateWorkflowOwnerCommand command)
        {
            if (state.InstanceOwnerId == Guid.Empty)
                throw new InvalidOperationException("InstanceOwnerId is empty.");

            context.BindInstanceOwner(state.InstanceOwnerId, Guid.NewGuid());

            return Task.FromResult(true);
        }

        /// <summary>
        /// Handles a <see cref="QueryActivatableWorkflowsCommand"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<bool> QueryActivatableWorkflowsCommand(InstancePersistenceContext context, QueryActivatableWorkflowsCommand command)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Handles a <see cref="SaveWorkflowCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        Task<bool> SaveWorkflowCommand(InstancePersistenceContext context, SaveWorkflowCommand command)
        {
            // save the instance data
            state.InstanceState = InstanceState.Initialized;
            SaveInstanceData(context.InstanceView.InstanceId, command.InstanceData);
            SaveInstanceMetadata(context.InstanceView.InstanceId, command.InstanceMetadataChanges);
            state.OnPersisted();

            // clear instance data when complete
            if (command.CompleteInstance)
            {
                state.InstanceState = InstanceState.Completed;
                state.ClearInstanceData();
                state.ClearInstanceMetadata();
                state.OnCompleted();
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Handles a <see cref="LoadWorkflowCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        Task<bool> LoadWorkflowCommand(InstancePersistenceContext context, LoadWorkflowCommand command)
        {
            if (state.InstanceId != context.InstanceView.InstanceId)
                throw new InvalidOperationException("Loading InstanceId does not match state InstanceId.");

            if (state.InstanceState != InstanceState.Completed)
            {
                context.LoadedInstance(
                    InstanceState.Initialized,
                    LoadInstanceData(context.InstanceView.InstanceId),
                    LoadInstanceMetadata(context.InstanceView.InstanceId),
                    null,
                    null);

                return Task.FromResult(true);
            }
            else
                return Task.FromResult(false);
        }

        /// <summary>
        /// Handles a <see cref="TryLoadRunnableWorkflowCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<bool> TryLoadRunnableWorkflowCommand(InstancePersistenceContext context, TryLoadRunnableWorkflowCommand command)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Handles a <see cref="DeleteWorkflowOwnerCommand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        Task<bool> DeleteWorkflowOwnerCommand(InstancePersistenceContext context, DeleteWorkflowOwnerCommand command)
        {
            return Task.FromResult(true);
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Loads the instance data for the given instance ID.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        IDictionary<XName, InstanceValue> LoadInstanceData(Guid instanceId)
        {
            return state.GetInstanceDataNames().ToDictionary(i => (XName)i, i => new InstanceValue(FromSerializableObject(state.GetInstanceData(i))));
        }

        /// <summary>
        /// Lodas the instance metadata for the given instance ID.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        IDictionary<XName, InstanceValue> LoadInstanceMetadata(Guid instanceId)
        {
            return state.GetInstanceMetadataNames().ToDictionary(i => (XName)i, i => new InstanceValue(FromSerializableObject(state.GetInstanceMetadata(i))));
        }

        /// <summary>
        /// Saves the instance data for the given instance ID.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="data"></param>
        void SaveInstanceData(Guid instanceId, IDictionary<XName, InstanceValue> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            foreach (var kvp in data)
                state.SetInstanceData(kvp.Key.ToString(), ToSerializableObject(kvp.Value.Value));
        }

        /// <summary>
        /// Saves the instance metadata for the given instance ID.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="metadata"></param>
        void SaveInstanceMetadata(Guid instanceId, IDictionary<XName, InstanceValue> metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            foreach (var kvp in metadata)
                state.SetInstanceMetadata(kvp.Key.ToString(), ToSerializableObject(kvp.Value.Value));
        }

        /// <summary>
        /// Serializes the given serializable object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ToSerializableObject(object value)
        {
            if (value.GetType().IsPrimitive)
                return value;
            else if (value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(Nullable<>) &&
                Nullable.GetUnderlyingType(value.GetType()).IsPrimitive)
                return value;
            else if (value is string)
                return value;
            else if (value.GetType().IsEnum)
                return value;
            else if (value is DateTime? || value is DateTime)
                return value;
            else if (value is DateTimeOffset? || value is DateTimeOffset)
                return value;
            else if (value is TimeSpan? || value is TimeSpan)
                return value;
            else if (value is Guid? || value is Guid)
                return value;
            else if (value is Uri)
                return value;
            else if (value is byte[])
                return value;
            else if (value is XmlQualifiedName)
                return value;
            else if (value is XName)
                return value;
            else if (value is ReadOnlyCollection<BookmarkInfo>)
                return value;
            else
                return new ActivityActorInstanceValueAsXml() { Value = SerializeObject(value) };
        }

        /// <summary>
        /// Serializes the given object to a <see cref="XElement"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        XElement SerializeObject(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var d1 = new XmlDocument();
            using (var w = d1.CreateNavigator().AppendChild())
                serializer.WriteObject(w, value);

            var d2 = new XDocument();
            using (var w = d2.CreateWriter())
                d1.DocumentElement.WriteTo(w);

            return d2.Root;
        }

        /// <summary>
        /// Deserializes the given serializable object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object FromSerializableObject(object value)
        {
            if (value is ActivityActorInstanceValueAsXml)
                return DeserializeObject(((ActivityActorInstanceValueAsXml)value).Value);
            else
                return value;
        }

        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        object DeserializeObject(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            using (var rdr = element.CreateReader())
                return serializer.ReadObject(rdr);
        }

        #endregion

    }

}
