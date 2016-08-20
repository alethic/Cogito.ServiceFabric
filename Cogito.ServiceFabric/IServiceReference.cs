using System;
using System.Diagnostics.Contracts;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Describes a reference to a service.
    /// </summary>
    [ContractClass(typeof(IServiceReference_Contract))]
    public interface IServiceReference
    {

        /// <summary>
        /// Converts the service reference into a service proxy.
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <returns></returns>
        object Bind(Type serviceInterfaceType);

    }

    [ContractClassFor(typeof(IServiceReference))]
    abstract class IServiceReference_Contract :
        IServiceReference
    {

        public object Bind(Type serviceInterfaceType)
        {
            Contract.Requires<ArgumentNullException>(serviceInterfaceType != null);
            throw new NotImplementedException();
        }

    }

}
