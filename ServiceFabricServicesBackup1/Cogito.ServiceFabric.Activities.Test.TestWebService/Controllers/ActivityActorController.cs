using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Cogito.ServiceFabric.Activities.Test.TestWebService.Controllers
{

    [RoutePrefix("activity-actor")]
    public class ActivityActorController :
        ApiController
    {

        [Route("test")]
        [HttpGet]
        public async Task<IHttpActionResult> Test()
        {
            var a = ActorProxy.Create<ITest>(new ActorId(Guid.NewGuid()));
            await a.CallMe();
            await a.Start();
            return Content(HttpStatusCode.OK, a.GetActorId());
        }

        [Route("test/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Test(Guid id)
        {
            var a = ActorProxy.Create<ITest>(new ActorId(id));
            await a.CallMe();
            return Content(HttpStatusCode.OK, a.GetActorId());
        }

    }

}
