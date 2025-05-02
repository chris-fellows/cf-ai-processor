using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class SystemTaskConfig
    {   
        /// <summary>
        /// System task name
        /// </summary>
        public string SystemTaskName { get; set; } = String.Empty;

        /// <summary>
        /// Execute frequency
        /// </summary>
        public TimeSpan ExecuteFrequency { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Next regular execute time
        /// </summary>
        public DateTimeOffset NextExecuteTime { get; set; }
    }
}
