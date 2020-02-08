using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.AspNetCore.Test.TestWebService.NetCore3
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
