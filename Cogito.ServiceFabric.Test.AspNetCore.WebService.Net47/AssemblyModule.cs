using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.Net47
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
