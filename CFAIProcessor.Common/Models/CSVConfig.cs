namespace CFAIProcessor.Models
{
    /// <summary>
    /// Configuration of a CSV file
    /// </summary>
    public class CSVConfig
    {
        public List<CSVColumnConfig> Columns { get; set; } = new();
    }
}
