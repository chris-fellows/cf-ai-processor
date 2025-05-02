namespace CFAIProcessor.Models
{
    /// <summary>
    /// Configuration of a CSV file
    /// </summary>
    internal class CSVConf
    {
        public List<CSVColumnConfig> Columns { get; set; } = new();
    }
}
