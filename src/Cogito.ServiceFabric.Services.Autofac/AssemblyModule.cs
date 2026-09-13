using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Services.Autofac
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.ServiceFabric.Autofac.AssemblyModule>();

            if (FabricEnvironment.IsFabric)
                builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
