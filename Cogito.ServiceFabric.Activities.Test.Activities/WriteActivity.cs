using System;
using System.Activities;

namespace Cogito.ServiceFabric.Activities.Test.Activities
{

    public class WriteActivity :
        CodeActivity
    {

        protected override void Execute(CodeActivityContext context)
        {
            Console.WriteLine("foo");
        }

    }

}
