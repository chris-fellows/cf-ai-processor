using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ChartConfig
    {
        //public string DataSource { get; set; } = String.Empty;

        public DataSetInfo DataSetInfo { get; set; }

        public List<ChartConfigAxisGroup> AxisGroups { get; set; } = new();
    }
}
