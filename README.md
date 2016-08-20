Ever wanted to run Windows Workflow activities inside Azure Service Fabric? Yeah, I know. Crazy.
But think about it. You could code your Actor's execution logic to be stateless, and support resuming later. For very long term operations. Or for automatically dealing with failover. Actor dies? It just pops up on another box and execution logic resumes where it left off.
I wrote this library to do just that.
The basic idea is that each Actor hosts a workflow that starts when the Actor is activated. You can interact with it using bookmarks. Persistence is accomplished through Actor state. Workflow threads are dispatched to the Actor's reentrancy context (by default). Long term idle is accomplished through reminders.
It solves the same set of problems Workflow Service Host does in WCF: exposing a workflow to an endpoint, managing it's lifecycle, dealing with persistence and long term wake up. But it does it using Service Fabric Actors. Which is really the perfect environment for it. Each running instance is it's own Actor instance. No shared SQL server for state. No single point of failure.
It's fairly simple to use. Just extend ActivityActor (or ActivityActor<T>), and implement CreateActivity() to return an Activity instance, and when the Actor is activated, it will begin running.
It also comes along with a helpful set of static methods to make coding Activities much easier. Witness this full example of an Actor that wakes up every 15 minutes to do some work:
using Cogito.ServiceFabric.Activities
using static Cogito.Activities.Expressions;

public TestActor : ActivityActor, ITestActor
{

    protected override Activity CreateActivity()
    {
        return Sequence(
            Wait("Start"),
            While(true,
                Sequence(
                    Delay(TimeSpan.FromMinutes(15)),
                    Invoke(async () => async DoWork())));
    }

    public Task Begin()
    {
        return ResumeAsync("Start");
    }

    Task DoWork()
    {
        Debug("Work");
        return Task.FromResult(true);
    }

}
You could override CreateActivity to load an Activity designed in the visual designer as XAML, too, of course.
That's it. Just install that Actor in Service Fabric, create an ActorProxy to it, invoke Begin and it will start doing its work.
I know Windows Workflow is generally seen as a pariah. But it actually is amazing. But it's UGLY, and it's internals are God awfully complicated. But a good execution environment, and some syntax help, can really make it shine. So I took care of most of that for you.
Available in NuGet!
https://www.nuget.org/packages/Cogito.ServiceFabric.Activities/
