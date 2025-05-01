namespace CFAIProcessor.Models
{
    /// <summary>
    /// Defines config for a group of CSV rows. The group may have specific range for each column value.
    /// </summary>
    internal class CSVRowGroup
    {        
        public string Name { get; set; } = String.Empty;

        public List<ColumnConfig> ColumnConfigs { get; set; } = new List<ColumnConfig>();

        public ColumnConfig ColumnConfig(string name) => ColumnConfigs.First(c => c.Name == name);        
    }
}
