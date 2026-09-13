using System;

using Microsoft.ServiceFabric.Services.Remoting;

namespace Cogito.ServiceFabric.Services
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
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return ServiceReference.Get(service);
        }

    }

}
