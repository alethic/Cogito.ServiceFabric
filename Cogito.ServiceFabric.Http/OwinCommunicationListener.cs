using System;
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
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));
            if (serviceContext == null)
                throw new ArgumentNullException(nameof(serviceContext));
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
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));
            if (serviceContext == null)
                throw new ArgumentNullException(nameof(serviceContext));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="appRoot"></param>
        /// <param name="configure"></param>
        /// <param name="serviceContext"></param>
        /// <param name="restartOnConfigurationPackageChange"></param>
        public OwinCommunicationListener(
            Action<IAppBuilder> configure,
            ServiceContext serviceContext,
            string endpointName,
            string appRoot,
            bool restartOnConfigurationPackageChange = false)
        {
            this.configure = configure ?? throw new ArgumentNullException(nameof(configure));
            this.serviceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
            this.endpointName = endpointName ?? throw new ArgumentNullException(nameof(endpointName));
            this.appRoot = appRoot;

            // trigger a web server restart if told to when the configuration package changes
            if (restartOnConfigurationPackageChange)
            {
                serviceContext.CodePackageActivationContext.ConfigurationPackageAddedEvent += (s, a) => RestartWebServerSync();
                serviceContext.CodePackageActivationContext.ConfigurationPackageModifiedEvent += (s, a) => RestartWebServerSync();
                serviceContext.CodePackageActivationContext.ConfigurationPackageRemovedEvent += (s, a) => RestartWebServerSync();
            }
        }

        /// <summary>
        /// Restarts the web server, synchronously.
        /// </summary>
        void RestartWebServerSync()
        {
            RestartWebServer(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Opens the communication channel.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            return await StartWebServer(cancellationToken);
        }

        /// <summary>
        /// Closes the communication channel.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            await StopWebServer(cancellationToken);
        }

        /// <summary>
        /// Aborts the communication channel.
        /// </summary>
        public void Abort()
        {
            StopWebServer(CancellationToken.None);
        }

        /// <summary>
        /// Starts the web server. Returns the address the service is listening on.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> StartWebServer(CancellationToken cancellationToken)
        {
            if (serverHandle != null)
                throw new InvalidOperationException("Web server is already started.");

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
            catch (Exception)
            {
                await StopWebServer(cancellationToken);
                throw;
            }
        }

        /// <summary>
        /// Stops the running web server.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task StopWebServer(CancellationToken cancellationToken)
        {
            if (serverHandle != null)
            {
                try
                {
                    serverHandle.Dispose();
                    listeningAddress = null;
                    publishAddress = null;
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

            return Task.FromResult(true);
        }

        /// <summary>
        /// Restarts the listening web server.
        /// </summary>
        /// <returns></returns>
        public async Task RestartWebServer(CancellationToken cancellationToken)
        {
            if (serverHandle != null)
            {
                await StopWebServer(cancellationToken);
                await StartWebServer(cancellationToken);
            }
        }

        /// <summary>
        /// Gets the service listener URI.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        string GetUri(string host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));

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
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (host == null)
                throw new ArgumentNullException(nameof(host));

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
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (host == null)
                throw new ArgumentNullException(nameof(host));

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
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

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

    }

}