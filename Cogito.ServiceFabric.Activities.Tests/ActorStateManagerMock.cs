using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;

namespace Cogito.ServiceFabric.Activities.Tests
{

    class ActorStateManagerMock :
        IActorStateManager
    {

        readonly Dictionary<string, object> store = new Dictionary<string, object>();

        public Task<T> AddOrUpdateStateAsync<T>(string stateName, T addValue, Func<string, T, T> updateValueFactory, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (store.ContainsKey(stateName))
                store[stateName] = updateValueFactory(stateName, (T)store[stateName]);
            else
                store[stateName] = addValue;

            return Task.FromResult((T)store[stateName]);
        }

        public Task AddStateAsync<T>(string stateName, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            store[stateName] = value;
            return Task.FromResult(true);
        }

        public Task ClearCacheAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        public Task<bool> ContainsStateAsync(string stateName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(store.ContainsKey(stateName));
        }

        public Task<T> GetOrAddStateAsync<T>(string stateName, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (store.ContainsKey(stateName))
                return Task.FromResult((T)store[stateName]);
            else
                return Task.FromResult((T)(store[stateName] = value));
        }

        public Task<T> GetStateAsync<T>(string stateName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult((T)store[stateName]);
        }

        public Task<IEnumerable<string>> GetStateNamesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(store.Keys.AsEnumerable());
        }

        public Task RemoveStateAsync(string stateName, CancellationToken cancellationToken = default(CancellationToken))
        {
            store.Remove(stateName);
            return Task.FromResult(true);
        }

        public Task SetStateAsync<T>(string stateName, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            store[stateName] = value;
            return Task.FromResult(true);
        }

        public Task<bool> TryAddStateAsync<T>(string stateName, T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (store.ContainsKey(stateName))
                return Task.FromResult(false);

            store[stateName] = value;
            return Task.FromResult(true);
        }

        public Task<ConditionalValue<T>> TryGetStateAsync<T>(string stateName, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (store.ContainsKey(stateName))
                return Task.FromResult(new ConditionalValue<T>(true, (T)store[stateName]));
            else
                return Task.FromResult(new ConditionalValue<T>(false, default(T)));
        }

        public Task<bool> TryRemoveStateAsync(string stateName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(store.Remove(stateName));
        }

    }

}
