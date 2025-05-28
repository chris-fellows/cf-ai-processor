using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ImageClassifyConfig
    {
        public string TestImageSetInfoId { get; set; } = String.Empty;

        public string TestImageSetInfoDataSource { get; set; } = String.Empty;

        public string ModelFolder { get; set; } = String.Empty;

        public string UserId { get; set; } = String.Empty;
    }
}
