using Microsoft.Extensions.Configuration;

namespace Cogito.ServiceFabric.Configuration
{

    public static class ServiceFabricConfigurationExtensions
    {

        /// <summary>
        /// Registers the given package name as configuration.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddServiceFabricConfig(this IConfigurationBuilder builder, string packageName)
        {
            return builder.Add(new ServiceFabricConfigurationSource(packageName));
        }

    }

}
