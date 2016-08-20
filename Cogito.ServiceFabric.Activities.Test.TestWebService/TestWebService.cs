using System.Fabric;
using System.Web.Http;
using Owin;

namespace Cogito.ServiceFabric.Activities.Test.TestWebService
{

    sealed class TestWebService :
        Cogito.ServiceFabric.Http.OwinStatelessService
    {

        public TestWebService(StatelessServiceContext context)
            : base(context, "cogito-activities-test")
        {

        }

        protected override void Configure(global::Owin.IAppBuilder appBuilder)
        {
            var http = new HttpConfiguration();
            http.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(http);
        }

    }

}
