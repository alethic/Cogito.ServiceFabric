using System.Activities;
using System.Diagnostics;
using System.Threading.Tasks;

using Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

using static Cogito.Activities.Expressions;

namespace Cogito.ServiceFabric.Activities.Test.TestActor
{

    [ActorService(Name = "TestActorService")]
    [StatePersistence(StatePersistence.Persisted)]
    class Test :
        ActivityActor<TestState>,
        ITest
    {

        public Test(ActorService actorService, ActorId actorId) :
            base(actorService, actorId)
        {

        }

        protected override Activity CreateActivity()
        {
            return Sequence(
                Wait("Start"));
        }

        async Task DoThing1()
        {
            Debug.WriteLine($"Test {Id} DoThing1");
            await Task.Delay(100);
            Debug.WriteLine($"Test {Id} DoThing1 End");
        }

        async Task DoThing2()
        {
            Debug.WriteLine($"Test {Id} DoThing2");
            await Task.Delay(100);
            Debug.WriteLine($"Test {Id} DoThing2 End");
        }

        public async Task CallMe()
        {
            Debug.WriteLine($"Test {Id} CallMe");
            for (int i = 0; i < 0; i++)
            {
                var a = ActorProxy.Create<ITest2>(ActorId.CreateRandom());
                await a.CallMe1(this);
                await a.CallMe2();
            }
            Debug.WriteLine($"Test {Id} CallMe End");
        }

        public async Task CallMeBack(ITest2 from)
        {
            Debug.WriteLine($"Test {Id} CallMeBack");
            await ResumeAsync("ThingPassed");
            Debug.WriteLine($"Test {Id} CallMeBack End");
        }

        public async Task Start()
        {
            Debug.WriteLine($"Test {Id} Start");
            await ResumeAsync("Start");
            Debug.WriteLine($"Test {Id} Start End");
        }

        protected override async Task OnIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            await base.OnIdleAsync(args);
            Debug.WriteLine("OnIdle");
        }

        protected override async Task OnPersistableIdleAsync(WorkflowApplicationIdleEventArgs args)
        {
            await base.OnPersistableIdleAsync(args);
            Debug.WriteLine("OnPersistableIdleAsync");
        }

        protected override Task OnUnhandledExceptionAsync(WorkflowApplicationUnhandledExceptionEventArgs args)
        {
            return base.OnUnhandledExceptionAsync(args);
        }

    }

}
