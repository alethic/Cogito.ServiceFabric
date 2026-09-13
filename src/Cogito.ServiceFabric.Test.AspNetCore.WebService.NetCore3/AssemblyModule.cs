using Autofac;

using Cogito.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.NetCore3
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.Extensions.Logging.Serilog.Autofac.AssemblyModule>();
            builder.RegisterModule<Cogito.ServiceFabric.Configuration.Autofac.AssemblyModule>();
            builder.RegisterModule<Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac.AssemblyModule>();
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
