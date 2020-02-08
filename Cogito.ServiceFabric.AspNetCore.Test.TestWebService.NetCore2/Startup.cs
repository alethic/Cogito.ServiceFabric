using System;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Cogito.Autofac;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cogito.ServiceFabric.AspNetCore.Test.TestWebService.NetCore2
{

    [RegisterAs(typeof(Startup))]
    public class Startup
    {

        readonly ILifetimeScope parent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public Startup(ILifetimeScope parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        /// <summary>
        /// Registers framework dependencies.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return new AutofacServiceProvider(parent.BeginLifetimeScope(builder => builder.Populate(services)));
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            app.Use(async (ctx, next) =>
            {
                await ctx.Response.WriteAsync("Hello World!");
            });
        }

    }

}
