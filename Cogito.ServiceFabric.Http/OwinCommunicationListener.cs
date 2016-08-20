using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

using Owin;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Implements a Service Fabric <see cref="ICommunicationListener"/> that handles incoming OWIN requests.
    /// </summary>
    public class OwinCommunicationListener :
        ICommunicationListener
    {

        readonly Action<IAppBuilder> configure;
        readonly ServiceContext serviceContext;
        readonly string endpointName;
        readonly string appRoot;

        string listeningAddress;
        string publishAddress;
        IDisposable serverHandle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="serviceContext"></param>
        public OwinCommunicationListener(
            Action<IAppBuilder> configure,
            ServiceContext serviceContext)
            : this(configure, serviceContext, null)
        {
            Contract.Requires<ArgumentNullException>(configure != null);
            Contract.Requires<ArgumentNullException>(serviceContext != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="serviceContext"></param>
        /// <param name="appRoot"></param>
        public OwinCommunicationListener(
            Action<IAppBuilder> configure,
            ServiceContext serviceContext,
            string appRoot)
            : this(configure, serviceContext, "HttpServiceEndpoint", appRoot)
        {
            Contract.Requires<ArgumentNullException>(configure != null);
            Contract.Requires<ArgumentNullException>(serviceContext != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="appRoot"></param>
        /// <param name="configure"></param>
        /// <param name="serviceContext"></param>
        public OwinCommunicationListener(
            Action<IAppBuilder> configure,
            ServiceContext serviceContext,
            string endpointName,
            string appRoot)
        {
            Contract.Requires<ArgumentNullException>(configure != null);
            Contract.Requires<ArgumentNullException>(serviceContext != null);
            Contract.Requires<ArgumentNullException>(endpointName != null);

            this.configure = configure;
            this.serviceContext = serviceContext;
            this.endpointName = endpointName;
            this.appRoot = appRoot;
        }

        ///// <summary>
        ///// Gets or sets whether the communication listener listens when the node is a secondary.
        ///// </summary>
        //public bool ListenOnSecondary { get; set; }

        /// <summary>
        /// Opens the communication channel.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            // obtain node context
            var nodeContext = await FabricRuntime.GetNodeContextAsync(TimeSpan.FromSeconds(15), cancellationToken);
            if (nodeContext == null)
                throw new FabricServiceNotFoundException("Could not obtain node context.");

            // listening address has +, publish address has real node address
            listeningAddress = GetUri("+");
            publishAddress = GetUri(nodeContext.IPAddressOrFQDN);

            try
            {
                serverHandle = WebApp.Start(listeningAddress, configure);
                return publishAddress;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);

                StopWebServer();

                throw;
            }
        }

        /// <summary>
        /// Gets the service listener URI.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        string GetUri(string host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            if (serviceContext is StatefulServiceContext)
                return GetUri((StatefulServiceContext)serviceContext, host);

            if (serviceContext is StatelessServiceContext)
                return GetUri((StatelessServiceContext)serviceContext, host);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the service listener URI.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        string GetUri(StatefulServiceContext context, string host)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(host != null);

            var serviceEndpoint = context.CodePackageActivationContext.GetEndpoint(endpointName);
            if (serviceEndpoint == null)
                throw new NullReferenceException(endpointName);

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}://{1}:{2}/{3}{4}/{5}",
                GetSchema(serviceEndpoint),
                host,
                serviceEndpoint.Port,
                string.IsNullOrWhiteSpace(appRoot) ? string.Empty : appRoot.TrimEnd('/') + '/',
                context.PartitionId,
                context.ReplicaId,
                Guid.NewGuid());
        }

        /// <summary>
        /// Gets the service listener URI.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        string GetUri(StatelessServiceContext context, string host)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(host != null);

            var serviceEndpoint = context.CodePackageActivationContext.GetEndpoint(endpointName);
            if (serviceEndpoint == null)
                throw new NullReferenceException(endpointName);

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}://{1}:{2}/{3}",
                GetSchema(serviceEndpoint),
                host,
                serviceEndpoint.Port,
                string.IsNullOrWhiteSpace(appRoot) ? string.Empty : appRoot.TrimEnd('/') + '/');
        }

        /// <summary>
        /// Gets the schema for the given <see cref="EndpointResourceDescription"/>.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        string GetSchema(EndpointResourceDescription endpoint)
        {
            Contract.Requires<ArgumentNullException>(endpoint != null);

            switch (endpoint.Protocol)
            {
                case EndpointProtocol.Http:
                    return "http";
                case EndpointProtocol.Https:
                    return "https";
                default:
                    throw new FabricEndpointNotFoundException($"Unsupported endpoint protocol for {nameof(OwinCommunicationListener)}.");
            }
        }

        /// <summary>
        /// Closes the communication channel.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            StopWebServer();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Aborts the communication channel.
        /// </summary>
        public void Abort()
        {
            StopWebServer();
        }

        /// <summary>
        /// Stops the running web server.
        /// </summary>
        void StopWebServer()
        {
            if (serverHandle != null)
            {
                try
                {
                    serverHandle.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // no-op
                }
                finally
                {
                    serverHandle = null;
                }
            }
        }

    }

}