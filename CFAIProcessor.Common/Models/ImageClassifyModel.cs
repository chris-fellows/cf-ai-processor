using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ImageClassifyModel
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string DataSetInfoId { get; set; } = String.Empty;

        public string ModelFolder { get; set; } = String.Empty;

        public ImageTrainConfig TrainConfig { get; set; } = new();

        public string CreatedUserId { get; set; } = String.Empty;
    }
}
