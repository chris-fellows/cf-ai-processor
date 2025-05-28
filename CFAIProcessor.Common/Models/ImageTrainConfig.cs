using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ImageTrainConfig
    {          
        public string TrainImageSetInfoId { get; set; } = String.Empty;

        public string TrainImageSetInfoDataSource { get; set; } = String.Empty;

        public string ValidImageSetInfoId { get; set; } = String.Empty;

        public string ValidImageSetInfoDataSource { get; set; } = String.Empty;

        public string ModelName { get; set; } = String.Empty;

        public string ModelFolder { get; set; } = String.Empty;


        [Range(1, Int32.MaxValue)]
        public int TrainingEpochs { get; set; }

        //public List<string> ClassNames { get; set; } = new();

        public string UserId { get; set; } = String.Empty;
    }
}
