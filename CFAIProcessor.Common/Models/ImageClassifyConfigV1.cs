using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ImageClassifyConfigV1
    {
        public string AllImageFolder { get; set; } = String.Empty;

        /// <summary>
        /// Folder containing images to train model
        /// </summary>
        public string TrainImageFolder { get; set; } = String.Empty;

        /// <summary>
        /// Folder containing images to test model
        /// </summary>
        public string TestImageFolder { get; set; } = String.Empty;

        public string ValidImageFolder { get; set; } = String.Empty;

        public int TrainImageCount { get; set; }

        public int ValidImageCount { get; set; }

        public int TestImageCount { get; set; }        

        public int TrainEpochs { get; set; } = 10;

        public List<string> ClassNames { get; set; } = new();
    }
}
