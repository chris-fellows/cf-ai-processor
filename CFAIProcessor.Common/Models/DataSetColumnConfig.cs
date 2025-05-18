using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class DataSetColumnConfig
    {  
        /// <summary>
       /// Name in CSV file
       /// </summary>
        public string InternalName { get; set; } = String.Empty;

        /// <summary>
        /// Display name
        /// </summary>
        public string ExternalName { get; set; } = String.Empty;

        /// <summary>
        /// Min value
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Max value
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Whether column is a feature for predictions
        /// </summary>
        public bool IsFeature { get; set; }

        /// <summary>
        /// Whether column is a label for predictions
        /// </summary>
        public bool IsLabel { get; set; }
    }
}
