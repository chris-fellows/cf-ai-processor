using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.SystemTask
{
    public interface ISystemTaskList
    {
        /// <summary>
        /// Max number of concurrent tasks allowed
        /// </summary>
        int MaxConcurrentTasks { get; }

        ///// <summary>
        ///// System tasks available
        ///// </summary>
        //IReadOnlyList<ISystemTask> SystemTasks { get; }

        /// <summary>
        /// Active system tasks
        /// </summary>
        IReadOnlyList<SystemTaskActive> SystemTaskActives { get; }

        /// <summary>
        /// System task requests for one off execute
        /// </summary>
        IReadOnlyList<SystemTaskRequest> SystemTaskRequests { get; }

        /// <summary>
        /// Returns next overdue regular system task
        /// </summary>
        /// <returns></returns>
        SystemTaskConfig? GetNextRegularOverdue();

        /// <summary>
        /// Returns next overdue requested system task
        /// </summary>
        /// <returns></returns>
        SystemTaskRequest? GetNextRequestedOverdue();

        /// <summary>
        /// Adds request to execute system task
        /// </summary>
        /// <param name="systemTaskRequest"></param>
        void AddRequest(SystemTaskRequest systemTaskRequest);

        /// <summary>
        /// Sets system task as active
        /// </summary>
        /// <param name="systemTask"></param>
        /// <param name="task"></param>
        void SetActive(ISystemTask systemTask, Task task, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Sets system task as complete
        /// </summary>
        /// <param name="systemTask"></param>
        void SetComplete(ISystemTask systemTask);

        /// <summary>
        /// Cancel system task. Waits for task to abort.
        /// </summary>
        /// <param name="systemTask"></param>
        void Cancel(ISystemTask systemTask);
    }
}
