using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Configuration.Autofac
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.Extensions.Configuration.Autofac.AssemblyModule>();
            builder.RegisterModule<Cogito.ServiceFabric.Autofac.AssemblyModule>();

            if (FabricEnvironment.IsFabric)
                builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
