using Cogito.Autofac;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cogito.ServiceFabric.AspNetCore.Test.TestWebService.NetCore3
{

    [RegisterAs(typeof(Startup))]
    public class Startup
    {

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                await ctx.Response.WriteAsync("Hello World!");
            });
        }

    }

}
