using System;
using System.Fabric;

using Cogito.Autofac;
using Cogito.Extensions.Configuration.Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.AspNetCore.Configuration;

namespace Cogito.ServiceFabric.Configuration.Autofac
{

    /// <summary>
    /// Applies available configuration packages to the configuration framework.
    /// </summary>
    [RegisterAs(typeof(IConfigurationBuilderConfiguration))]
    public class ServiceFabricConfigurationBuilderConfiguration : IConfigurationBuilderConfiguration
    {

        public IConfigurationBuilder Apply(IConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (FabricEnvironment.IsFabric)
                builder = builder.AddServiceFabricConfiguration(FabricRuntime.GetActivationContext(), o => o.IncludePackageName = false);

            return builder;
        }

    }

}
