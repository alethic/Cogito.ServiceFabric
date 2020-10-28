namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Describes a default service endpoint to be provided to a service.
    /// </summary>
    public class DefaultServiceEndpoint
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        public DefaultServiceEndpoint(string endpointName)
        {
            EndpointName = endpointName;
        }

        /// <summary>
        /// Name of the endpoint resource.
        /// </summary>
        public string EndpointName { get; private set; }

    }

}
