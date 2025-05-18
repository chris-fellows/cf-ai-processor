using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class PredictionTrainConfig
    {
        public string DataSetInfoId { get; set; } = String.Empty;

        [Range(1, Int32.MaxValue)]
        public int TrainingEpochs { get; set; } = 1000;

        [Range(0.000001, float.MaxValue)]
        public float LearningRate { get; set; } = 0.01f;

        public bool NormaliseValues { get; set; } = true;

        public string ModelName { get; set; } = String.Empty;

        public string ModelFolder { get; set; } = String.Empty;

        public int MaxTrainRows { get; set; } = Int32.MaxValue;

        public List<DataSetColumnConfig> ColumnConfigs { get; set; } = new();

        public string UserId { get; set; } = String.Empty;
    }
}
