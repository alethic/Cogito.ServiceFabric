using System;
using System.Diagnostics.Contracts;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Describes a reference to a service.
    /// </summary>
    public interface IServiceReference
    {

        /// <summary>
        /// Converts the service reference into a service proxy.
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <returns></returns>
        object Bind(Type serviceInterfaceType);

    }

}
