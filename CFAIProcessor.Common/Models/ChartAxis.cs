using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ChartAxis
    {
        public string Name { get; set; } = String.Empty;

        public object[] Values { get; set; } = new object[0];
    }
}
