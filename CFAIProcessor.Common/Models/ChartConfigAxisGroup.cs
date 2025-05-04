using CFAIProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ChartConfigAxisGroup
    {
        public AxisGroupModes Mode { get; set; } = AxisGroupModes.Markers;

        public string Color { get; set; } = String.Empty;

        public List<string> AxisColumns { get; set; } = new();
    }
}
