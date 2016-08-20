using System;
using System.Activities;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Cogito.ServiceFabric.Activities.Test.TestActor.Interfaces;

using Microsoft.ServiceFabric.Actors.Runtime;

using static Cogito.Activities.Expressions;

namespace Cogito.ServiceFabric.Activities.Test.TestActor
{

    [ActorService(Name = "Test2ActorService")]
    [StatePersistence(StatePersistence.Persisted)]
    class Test2 :
        ActivityActor<Test2State>,
        ITest2
    {

        protected override Activity CreateActivity()
        {
            return Sequence(
                Wait("Start"),
                Invoke(() => DoThing1()),
                While(true,
                    Sequence(
                        Delay(TimeSpan.FromSeconds(5)),
                        Invoke(() => DoThing2()))));
        }

        async Task DoThing1()
        {
            Debug.WriteLine($"Test2 {Id} DoThing1");
            await Task.Delay(100);
            Debug.WriteLine($"Test2 {Id} DoThing1 End");
        }

        async Task DoThing2()
        {
            Debug.WriteLine($"Test2 {Id} DoThing2");
            await Task.Delay(100);
            Debug.WriteLine($"Test2 {Id} DoThing2 End");
        }

        public async Task CallMe1(ITest test)
        {
            Debug.WriteLine($"Test2 {Id} CallMe1");
            State.Others.Add(test);
            Debug.WriteLine($"Test2 {Id} CallMe1 End");
        }

        public async Task CallMe2()
        {
            Debug.WriteLine($"Test2 {Id} CallMe2");
            foreach (var i in State.Others) 
            {
                await i.CallMeBack(this);
            }
            Debug.WriteLine($"Test2 {Id} CallMe2 End");
        }

        public async Task Start()
        {
            State.Element = new System.Xml.Linq.XElement("Foo", new XElement("Bar", new XElement("Blah")));
            Debug.WriteLine($"Test2 {Id} Start");
            await ResumeAsync("Start");
            Debug.WriteLine($"Test2 {Id} Start End");
        }

    }

}
