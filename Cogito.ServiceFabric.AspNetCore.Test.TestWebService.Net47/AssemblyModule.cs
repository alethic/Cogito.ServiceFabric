using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.AspNetCore.Test.TestWebService.Net47
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
