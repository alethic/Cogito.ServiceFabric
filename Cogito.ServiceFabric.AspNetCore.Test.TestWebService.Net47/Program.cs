using System.Threading;
using System.Threading.Tasks;

using Autofac;
using Autofac.Integration.ServiceFabric;

using Cogito.Autofac;
using Cogito.ServiceFabric.AspNetCore.Kestrel.Autofac;

namespace Cogito.ServiceFabric.AspNetCore.Test.TestWebService.Net47
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
            builder.RegisterServiceFabricSupport();
            builder.RegisterStatelessKestrelWebService<Startup>("Cogito.ServiceFabric.AspNetCore.Test.TestWebService.Net47");

            using (builder.Build())
                await Task.Delay(Timeout.Infinite);
        }

    }

}
