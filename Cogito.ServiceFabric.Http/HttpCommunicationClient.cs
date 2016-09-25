using System;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Net.Http;

using Microsoft.ServiceFabric.Services.Communication.Client;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Implements a <see cref="ICommunicationClient"/> for interacting with HTTP services.
    /// </summary>
    public class HttpCommunicationClient :
        ICommunicationClient
    {

        readonly HttpClient http;
        readonly Uri baseAddress;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseAddress"></param>
        public HttpCommunicationClient(Uri baseAddress)
        {
            Contract.Requires<ArgumentNullException>(baseAddress != null);

            this.http = new HttpClient();
            this.baseAddress = baseAddress;
        }

        /// <summary>
        /// Gets the <see cref="HttpClient"/> instance used to communicate with the service.
        /// </summary>
        public HttpClient Http
        {
            get { return http; }
        }

        /// <summary>
        /// Gets the endpoint address.
        /// </summary>
        public Uri BaseAddress
        {
            get { return baseAddress; }
        }

        /// <summary>
        /// Gets a relative URI to the endpoint address of the service.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Uri GetRelativeUri(string path)
        {
            Contract.Requires<ArgumentNullException>(path != null);

            return BaseAddress.Combine(path);
        }

        /// <summary>
        /// Gets or Sets the name of the listener in the replica or instance to which the client is connected to.
        /// </summary>
        string ICommunicationClient.ListenerName { get; set; }

        /// <summary>
        ///  Gets or Sets the service endpoint to which the client is connected to.
        /// </summary>
        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get; set; }

        /// <summary>
        /// Gets or Sets the Resolved service partition which was used when this client was created.
        /// </summary>
        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }

    }

}
