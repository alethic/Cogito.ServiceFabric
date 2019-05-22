using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Configuration.Autofac
{

    public class ServiceFabricModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            if (FabricEnvironment.IsFabric)
                builder.RegisterFromAttributes(typeof(ServiceFabricModule).Assembly);
        }

    }

}
