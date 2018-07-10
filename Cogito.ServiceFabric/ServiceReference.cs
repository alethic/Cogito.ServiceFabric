using System;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Encapsulation of a reference to a service for serialization.
    /// </summary>
    [DataContract]
    public class ServiceReference :
        IServiceReference
    {

        static readonly MethodInfo BindMethodInfo = typeof(ServiceReference).GetMethods()
            .Where(i => i.Name == nameof(Bind))
            .Where(i => i.IsGenericMethodDefinition)
            .First();

        /// <summary>
        /// Gets a <see cref="ServiceReference"/> for the service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ServiceReference Get(object service)
        {
            Contract.Requires<ArgumentNullException>(service != null);

            if (service is Microsoft.ServiceFabric.Services.Runtime.StatelessService)
                return Get((Microsoft.ServiceFabric.Services.Runtime.StatelessService)service);

            if (service is Microsoft.ServiceFabric.Services.Runtime.StatefulService)
                return Get((Microsoft.ServiceFabric.Services.Runtime.StatefulService)service);

            if (service is ServiceProxy)
                return Get((ServiceProxy)service);

            throw new FabricException($"Cannot generate a {nameof(ServiceReference)} for this object type.");
        }

        /// <summary>
        /// Gets a <see cref="ServiceReference"/> for the service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        static ServiceReference Get(ServiceProxy service)
        {
            Contract.Requires<ArgumentNullException>(service != null);

            return new ServiceReference()
            {
                ServiceUri = service.ServicePartitionClient2.ServiceUri,
                PartitionKind = service.ServicePartitionClient2.PartitionKey.Kind,
                PartitionKey = service.ServicePartitionClient2.PartitionKey.Value,
                ListenerName = service.ServicePartitionClient2.ListenerName,
            };
        }

        /// <summary>
        /// Gets a <see cref="ServiceReference"/> for the service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        static ServiceReference Get(Microsoft.ServiceFabric.Services.Runtime.StatelessService service)
        {
            Contract.Requires<ArgumentNullException>(service != null);

            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a <see cref="ServiceReference"/> for the service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        static ServiceReference Get(Microsoft.ServiceFabric.Services.Runtime.StatefulService service)
        {
            Contract.Requires<ArgumentNullException>(service != null);

            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        [DataMember]
        public Uri ServiceUri { get; set; }

        /// <summary>
        /// Gets the partition kind of the service.
        /// </summary>
        [DataMember]
        public ServicePartitionKind PartitionKind { get; set; }

        /// <summary>
        /// Gets the partition key. This can be casted to the right type based on the <see cref="PartitionKind"/> member.
        /// </summary>
        [DataMember]
        public object PartitionKey { get; set; }

        /// <summary>
        /// If there are multiple communication listeners in the service, this property identifies the endpoint to which
        /// the communication client needs to connect to.
        /// </summary>
        [DataMember]
        public string ListenerName { get; set; }

        /// <summary>
        /// Creates a <see cref="ServiceProxy"/>.
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <returns></returns>
        public object Bind(Type serviceInterfaceType)
        {
            return BindMethodInfo.MakeGenericMethod(serviceInterfaceType).Invoke(this, new object[0]);
        }

        /// <summary>
        /// Creates a <see cref="ServiceProxy"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Bind<TService>()
            where TService : IService
        {
            if (PartitionKind == ServicePartitionKind.Singleton)
                return ServiceProxy.Create<TService>(ServiceUri, listenerName: ListenerName);

            if (PartitionKind == ServicePartitionKind.Int64Range)
                return ServiceProxy.Create<TService>(ServiceUri, new ServicePartitionKey((long)PartitionKey), listenerName: ListenerName);

            if (PartitionKind == ServicePartitionKind.Named)
                return ServiceProxy.Create<TService>(ServiceUri, new ServicePartitionKey((string)PartitionKey), listenerName: ListenerName);

            throw new FabricException($"Unsupported PartitionKind '{PartitionKind}'.");
        }

    }

}
