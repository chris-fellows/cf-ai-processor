using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    /// <summary>
    /// Details of an image set
    /// </summary>
    public class ImageSetInfo
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Data source. E.g. ZIP file
        /// </summary>
        public string DataSource { get; set; } = String.Empty;
    }
}
