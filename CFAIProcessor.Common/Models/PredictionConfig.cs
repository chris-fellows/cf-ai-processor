namespace CFAIProcessor.Models
{
    /// <summary>
    /// Prediction config
    /// </summary>
    public class PredictionConfig
    {
        public string Name { get; set; } = String.Empty;

        public int? MaxTrainRows { get; set; }
        
        public int? MaxTestRows { get; set; }

        public int TrainingEpochs { get; set; } = 1000;

        public float LearningRate { get; set; } = 0.01f;        

        public bool NormaliseValues { get; set; } = true;

        public bool IsImportingGraph { get; set; } = false;

        public string ModelFolder { get; set; } = String.Empty;
    }
}
