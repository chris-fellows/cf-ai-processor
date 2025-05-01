using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    internal class CSVDataConfig<TEntityType>
    {
        public string File { get; set; }

        public Char Delimiter { get; set; }

        public int MaxRecords { get; set; }

        public Func<TEntityType> CreateRandomEntity { get; set; }

        public List<string> ColumnNames { get; set; }

        public Func<TEntityType, string[]> GetColumnValues { get; set; }
    }
}
