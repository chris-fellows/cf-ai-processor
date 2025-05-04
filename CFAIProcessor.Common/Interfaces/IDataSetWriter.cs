namespace CFAIProcessor.Interfaces
{
    /// <summary>
    /// Writes data to a data set. E.g. CSV file.    
    /// </summary>
    internal interface IDataSetWriter
    {
        void WriteRow(Dictionary<string, string> row);
    }
}
