namespace CFAIProcessor.Models
{
    public class ChartData
    {
        public string Title { get; set; } = String.Empty;

        public string ChartTypeId { get; set; } = String.Empty;

        public List<ChartAxisGroup> AxisGroups { get; set; } = new();
    }
}
