using Autofac;
using Autofac.Integration.ServiceFabric;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Autofac
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            if (FabricEnvironment.IsFabric)
            {
                builder.RegisterServiceFabricSupport();
                builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
            }
        }

    }

}
