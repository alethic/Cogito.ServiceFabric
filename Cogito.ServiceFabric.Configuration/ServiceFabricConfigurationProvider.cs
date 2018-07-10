using System.Fabric;
using System.Net;

using Microsoft.Extensions.Configuration;

namespace Cogito.ServiceFabric.Configuration
{

    /// <summary>
    /// Provides a given package name as configuration.
    /// </summary>
    public class ServiceFabricConfigurationProvider :
        ConfigurationProvider
    {

        readonly string packageName;
        readonly CodePackageActivationContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="packageName"></param>
        public ServiceFabricConfigurationProvider(string packageName)
        {
            this.packageName = packageName;

            context = FabricRuntime.GetActivationContext();
            context.ConfigurationPackageModifiedEvent += (sender, e) =>
            {
                LoadPackage(e.NewPackage, reload: true);
                OnReload();
            };
        }

        public override void Load()
        {
            LoadPackage(context.GetConfigurationPackageObject(packageName));
        }

        void LoadPackage(ConfigurationPackage config, bool reload = false)
        {
            if (reload)
                Data.Clear();

            foreach (var section in config.Settings.Sections)
                foreach (var param in section.Parameters)
                    Data[$"{section.Name}:{param.Name}"] = param.IsEncrypted ? new NetworkCredential("", param.DecryptValue()).Password : param.Value;
        }

    }

}
