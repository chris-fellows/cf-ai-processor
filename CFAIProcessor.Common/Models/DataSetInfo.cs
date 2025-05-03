using CFAIProcessor.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class DataSetInfo
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Data source. E.g. CSV file
        /// </summary>
        public string DataSource { get; set; } = String.Empty;

        public List<DataSetColumn> Columns { get; set; } = new();
    }
}
