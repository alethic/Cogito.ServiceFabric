using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.AspNetCore.Autofac
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.ServiceFabric.Services.Autofac.AssemblyModule>();

            if (FabricEnvironment.IsFabric)
                builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
