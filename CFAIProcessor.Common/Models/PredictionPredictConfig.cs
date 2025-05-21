namespace CFAIProcessor.Models
{
    /// <summary>
    /// Config for predict using prediction model
    /// </summary>
    public class PredictionPredictConfig
    {
        /// <summary>
        /// Data set for prediction
        /// </summary>
        public string DataSetInfoId { get; set; } = String.Empty;

        public string DataSetInfoDataSource { get; set; } = String.Empty;

        /// <summary>
        /// Whether to normalise values
        /// </summary>
        public bool NormaliseValues { get; set; } = true;

        public string ModelFolder { get; set; } = String.Empty;

        public int MaxTestRows { get; set; } = Int32.MaxValue;

        public List<DataSetColumnConfig> ColumnConfigs { get; set; } = new();

        public string UserId { get; set; } = String.Empty;
    }
}
