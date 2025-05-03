using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Common.Models
{
    public class ChartData
    {
        public string Title { get; set; } = String.Empty;

        public string ChartTypeId { get; set; } = String.Empty;

        public List<ChartAxisGroup> AxisGroups { get; set; } = new();
    }
}
