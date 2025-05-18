using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class DateRangeFilter
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public DateTimeOffset FromDate { get; set; } = DateTimeOffset.MinValue;

        public DateTimeOffset ToDate { get; set; } = DateTimeOffset.MaxValue;
    }
}
