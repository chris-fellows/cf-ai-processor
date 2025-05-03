using CFAIProcessor.Common.Models;
using CFAIProcessor.Models;

namespace CFAIProcessor.Interfaces
{
    /// <summary>
    /// Chart data service
    /// </summary>
    public interface IChartDataService
    {
        ChartData GetChartData(string chartTypeId, ChartConfig chartConfig);
    }
}
