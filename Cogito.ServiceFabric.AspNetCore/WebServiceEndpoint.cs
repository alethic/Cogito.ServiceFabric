namespace Cogito.ServiceFabric.AspNetCore
{

    /// <summary>
    /// Describes a web service endpoint.
    /// </summary>
    public class WebServiceEndpoint
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        public WebServiceEndpoint(string endpointName)
        {
            EndpointName = endpointName;
        }

        /// <summary>
        /// Name of the endpoint resource.
        /// </summary>
        public string EndpointName { get; private set; }

    }

}
