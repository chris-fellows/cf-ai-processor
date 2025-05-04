using CFAIProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class AggregateColumn
    {
        public string InputName { get; set; } = String.Empty;

        public string OutputName { get; set; } = String.Empty;

        public AggregateActions AggregateAction { get; set; } = AggregateActions.None;

        public int? DecimalPlaces { get; set; }

        public NumberConvertActions? NumberConvertAction { get; set; }

        public int? NumberConvertModuloValue { get; set; }

        public List<string> GroupByColumnInternalNames { get; set; } = new();
    }
}
