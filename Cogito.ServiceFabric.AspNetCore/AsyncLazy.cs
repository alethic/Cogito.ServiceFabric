using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Cogito.ServiceFabric.AspNetCore
{

    static class AsyncLazy
    {

        /// <summary>
        /// Creates a new instance of <see cref="AsyncLazy{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static AsyncLazy<T> Create<T>(Func<Task<T>> valueFactory)
        {
            return new AsyncLazy<T>(valueFactory);
        }

    }

    /// <summary>
    /// Provides an async implementation of <see cref="Lazy{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class AsyncLazy<T>
    {

        readonly Lazy<Task<T>> lazy;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="valueFactory"></param>
        public AsyncLazy(Func<Task<T>> valueFactory)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            lazy = new Lazy<Task<T>>(async () => await valueFactory(), true);
        }

        /// <summary>
        /// Gets an awaiter used to await this <see cref="AsyncLazy{T}"/>.
        /// </summary>
        /// <returns></returns>
        public TaskAwaiter<T> GetAwaiter()
        {
            return lazy.Value.GetAwaiter();
        }

    }

}
