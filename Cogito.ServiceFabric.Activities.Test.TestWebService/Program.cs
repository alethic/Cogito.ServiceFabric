using System;
using System.Threading;

using Microsoft.ServiceFabric.Services.Runtime;

namespace Cogito.ServiceFabric.Activities.Test.TestWebService
{

    static class Program
    {

        static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("TestWebServiceType", ctx => new TestWebService(ctx)).Wait();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }

}
