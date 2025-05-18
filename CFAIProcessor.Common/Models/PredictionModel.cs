namespace CFAIProcessor.Models
{
    /// <summary>
    /// Prediction model
    /// </summary>
    public class PredictionModel
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string DataSetInfoId { get; set; } = String.Empty;

        public string ModelFolder { get; set; } = String.Empty;

        public PredictionTrainConfig TrainConfig { get; set; } = new();

        public string CreatedUserId { get; set; } = String.Empty;        
    }
}
