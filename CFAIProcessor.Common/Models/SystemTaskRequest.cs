using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class SystemTaskRequest
    {
        public string SystemTaskName { get; internal set; }

        public DateTimeOffset ExecuteTime { get; internal set; }

        public Dictionary<string, object> Parameters { get; internal set; }

        public SystemTaskRequest(DateTimeOffset executeTime, Dictionary<string, object> parameters, string systemTaskName)
        {
            ExecuteTime = executeTime;
            Parameters = parameters;
            SystemTaskName = systemTaskName;
        }
    }
}
