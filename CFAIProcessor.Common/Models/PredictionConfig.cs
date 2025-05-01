using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    internal class PredictionConfig
    {
        public string Name { get; set; } = String.Empty;

        public string TrainDataFile { get; set; } = String.Empty;

        public string TestDataFile { get; set; } = String.Empty;

        public int TrainingEpochs { get; set; } = 1000;

        public float LearningRate { get; set; } = 0.01f;

        public bool Enabled { get; set; } = true;

        public bool IsImportingGraph { get; set; } = false;

        public string ModelFolder { get; set; } = String.Empty;
    }
}
