namespace CFAIProcessor.Models
{
    /// <summary>
    /// Defines config for a group of CSV rows. The group may have specific range for each column value.
    /// </summary>
    public class CSVRowGroup
    {        
        public string Name { get; set; } = String.Empty;        

        /// <summary>
        /// When group is being selected randomly then the approximate proportion of the time that item should be selected
        /// </summary>
        public double RandomSelectionProportion { get; set; }

        public List<CSVColumnConfig> ColumnConfigs { get; set; } = new List<CSVColumnConfig>();

        public CSVColumnConfig ColumnConfig(string name) => ColumnConfigs.First(c => c.InternalName == name || c.ExternalName == name);        
    }
}
