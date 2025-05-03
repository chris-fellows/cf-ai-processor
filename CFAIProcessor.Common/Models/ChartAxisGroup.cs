using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ChartAxisGroup
    {
        public string Name { get; set; } = String.Empty;

        public List<ChartAxis> AxisList { get; set; } = new List<ChartAxis>();
    }
}
