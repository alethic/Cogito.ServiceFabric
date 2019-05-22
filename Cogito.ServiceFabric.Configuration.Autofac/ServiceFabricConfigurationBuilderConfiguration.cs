using System;
using System.Fabric;

using Cogito.Autofac;
using Cogito.Extensions.Configuration.Autofac;

using Microsoft.Extensions.Configuration;

namespace Cogito.ServiceFabric.Configuration.Autofac
{

    /// <summary>
    /// Applies available configuration packages to the configuration framework.
    /// </summary>
    [RegisterAs(typeof(IConfigurationBuilderConfiguration))]
    public class ServiceFabricConfigurationBuilderConfiguration :
        IConfigurationBuilderConfiguration
    {

        public IConfigurationBuilder Apply(IConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var context = FabricRuntime.GetActivationContext();
            if (context != null)
                foreach (var config in context.GetConfigurationPackageNames())
                    builder = builder.AddServiceFabricConfig(config);

            return builder;
        }

    }

}
