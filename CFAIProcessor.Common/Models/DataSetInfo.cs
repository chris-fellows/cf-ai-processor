namespace CFAIProcessor.Models
{
    /// <summary>
    /// Details of a data set
    /// </summary>
    public class DataSetInfo
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Data source. E.g. CSV file
        /// </summary>
        public string DataSource { get; set; } = String.Empty;

        /// <summary>
        /// Data set columns
        /// </summary>
        public List<DataSetColumn> Columns { get; set; } = new();
    }
}
