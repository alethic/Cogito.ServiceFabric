using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cogito.ServiceFabric.Activities.Tests
{

    [TestClass]
    public class ActivityActorStateManagerTests
    {

        //[TestMethod]
        //public void Test_ActivityActorStateManager()
        //{
        //    Task.Run(async () =>
        //    {
        //        var instd = Guid.NewGuid();
        //        var actor = new ActorStateManagerMock();
        //        var state = new ActivityActorStateManager(() => actor);

        //        await state.SetInstanceId(Guid.NewGuid());
        //        await state.SetInstanceOwnerId(Guid.NewGuid());
        //        await state.SetInstanceData("test1", "test1");
        //        await state.SetInstanceData("test2", "test2");

        //        state = new ActivityActorStateManager(() => actor);
        //        Assert.AreEqual("test1", await state.GetInstanceData("test1"));
        //        Assert.AreEqual("test2", await state.GetInstanceData("test2"));
        //    }).Wait();
        //}

    }

}
