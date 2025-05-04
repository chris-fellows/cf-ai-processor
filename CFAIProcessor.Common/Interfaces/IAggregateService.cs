using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    internal interface IAggregateService
    {
        DataTable Aggregate(DataTable table, AggregateConfig aggregateConfig);
    }
}
