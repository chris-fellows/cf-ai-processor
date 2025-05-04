using Razorvine.Pickle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class AggregateConfig
    {
        public List<AggregateColumn> Columns { get; set; } = new();
    }
}
