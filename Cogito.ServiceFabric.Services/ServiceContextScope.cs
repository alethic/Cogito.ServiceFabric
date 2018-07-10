using System;
using System.Fabric;

namespace Cogito.ServiceFabric.Services
{

    /// <summary>
    /// Manages a Logical Call Context variable containing a stack of <see cref="ServiceContext"/> instances.
    /// </summary>
    public static class ServiceContextScope
    {

        /// <summary>
        /// Publishes a <see cref="ServiceContext"/> onto the stack.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDisposable Push(ServiceContext context)
        {
            return AsyncLocalStack<ServiceContext>.Push(context);
        }

        /// <summary>
        /// Gets the current <see cref="ServiceContext"/>.
        /// </summary>
        public static ServiceContext Current => AsyncLocalStack<ServiceContext>.Current;

    }

}
