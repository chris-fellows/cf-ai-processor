using CFAIProcessor.SystemTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class SystemTaskActive
    {
        public ISystemTask SystemTask { get; internal set; }

        public Task Task { get; internal set; }

        public CancellationTokenSource CancellationTokenSource { get; internal set; }

        public SystemTaskActive(ISystemTask systemTask, Task task, CancellationTokenSource cancellationTokenSource)
        {
            SystemTask = systemTask;
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }
    }
}
