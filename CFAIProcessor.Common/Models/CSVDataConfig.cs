using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    internal class CSVDataConfig<TEntityType>
    {
        public string ConfigFile { get; set; } = String.Empty;
        public string DataFile { get; set; } = String.Empty;

        public Char Delimiter { get; set; }

        public int MaxRecords { get; set; }

        public Func<TEntityType> CreateRandomEntity { get; set; }

        public List<CSVColumnConfig> Columns { get; set; } = new();

        public Func<TEntityType, string[]> GetColumnValues { get; set; }
    }
}
