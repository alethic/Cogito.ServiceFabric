using System;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Throw when an operation is attempted but the workflow is not in a compatible state.
    /// </summary>
    public class ActivityStateException :
        InvalidOperationException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ActivityStateException()
            : this("Activity state is invalid for the current operation.")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ActivityStateException(string message)
            : base(message)
        {

        }

    }

}
