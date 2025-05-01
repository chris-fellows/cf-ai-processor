using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Logging
{
    internal interface ISimpleLog
    {
        void Log(DateTimeOffset time, string type, string message);
    }
}
