using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    /// <summary>
    /// Config for training prediction model
    /// </summary>
    public class PredictionTrainConfig
    {
        /// <summary>
        /// Data set for training
        /// </summary>
        public string DataSetInfoId { get; set; } = String.Empty;

        [Range(1, Int32.MaxValue)]
        public int TrainingEpochs { get; set; } = 1000;

        [Range(0.000001, float.MaxValue)]
        public float LearningRate { get; set; } = 0.01f;

        /// <summary>
        /// Whether to normalist feature and label values
        /// </summary>
        public bool NormaliseValues { get; set; } = true;

        /// <summary>
        /// Model name to create
        /// </summary>
        public string ModelName { get; set; } = String.Empty;

        /// <summary>
        /// Folder where model is stored
        /// </summary>
        public string ModelFolder { get; set; } = String.Empty;

        /// <summary>
        /// Max rows to train
        /// </summary>
        public int MaxTrainRows { get; set; } = Int32.MaxValue;

        public List<DataSetColumnConfig> ColumnConfigs { get; set; } = new();

        public string UserId { get; set; } = String.Empty;
    }
}
