using Microsoft.Extensions.Configuration;

namespace Cogito.ServiceFabric.Configuration
{

    /// <summary>
    /// Provides sources of configuration information from a given package name.
    /// </summary>
    class ServiceFabricConfigurationSource :
        IConfigurationSource
    {

        readonly string packageName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="packageName"></param>
        public ServiceFabricConfigurationSource(string packageName)
        {
            this.packageName = packageName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(packageName);
        }

    }

}
