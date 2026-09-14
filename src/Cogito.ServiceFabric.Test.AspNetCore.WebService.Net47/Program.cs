using System.Threading;
using System.Threading.Tasks;

using Autofac;

using Cogito.Autofac;
using Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac;

namespace Cogito.ServiceFabric.Test.AspNetCore.WebService.Net47
{

    public static class Program
    {

        /// <summary>
        /// Main application entry point.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAllAssemblyModules();
            builder.RegisterStatelessKestrelWebService<Startup>("Cogito.ServiceFabric.Test.AspNetCore.WebService.Net47.TestDefaultStatelessKestrelWebService");
            builder.RegisterStatefulKestrelWebService<Startup>("Cogito.ServiceFabric.Test.AspNetCore.WebService.Net47.TestDefaultStatefulKestrelWebService");

            using (builder.Build())
                await Task.Delay(Timeout.Infinite);
        }

    }

}
