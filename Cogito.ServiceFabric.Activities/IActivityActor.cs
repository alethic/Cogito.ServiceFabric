using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Public interface for activity actors.
    /// </summary>
    public interface IActivityActor :
        IActor
    {

        /// <summary>
        /// Resumes the workflow with the given bookmark name and value.
        /// </summary>
        /// <param name="bookmarkName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task ResumeAsync(string bookmarkName, object value);

    }

}
