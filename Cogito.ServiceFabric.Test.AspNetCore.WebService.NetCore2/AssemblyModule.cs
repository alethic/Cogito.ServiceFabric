using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.NetCore2
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
