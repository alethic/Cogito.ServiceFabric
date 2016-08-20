using System;
using System.Diagnostics.Contracts;

using Microsoft.ServiceFabric.Services.Remoting;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Provides extension methods for working against service proxies.
    /// </summary>
    public static class ServiceProxyExtensions
    {

        /// <summary>
        /// Gets 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ServiceReference GetServiceReference<TService>(this TService service)
            where TService : IService
        {
            Contract.Requires<ArgumentNullException>(service != null);

            return ServiceReference.Get(service);
        }

    }

}
