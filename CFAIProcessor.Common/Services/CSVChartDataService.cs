using CFAIProcessor.Enums;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFCSV.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class CSVChartDataService : IChartDataService
    {
        public ChartData GetChartData(string chartTypeId, ChartConfig chartConfig)
        {            
            try
            {
                // Load data             
                var dataTable = new CSVDataTableReader().Read(chartConfig.DataSetInfo.DataSource);                

                var chartData = new ChartData();

                foreach (var chartConfigAxisGroup in chartConfig.AxisGroups)
                {
                    // Create axis group
                    var chartAxisGroup = new ChartAxisGroup()
                    {
                        Name = $"Group {chartConfig.AxisGroups.IndexOf(chartConfigAxisGroup) + 1}",
                        Mode = chartConfigAxisGroup.Mode,
                        Color = chartConfigAxisGroup.Color,
                    };
                    foreach (var axisColumn in chartConfigAxisGroup.AxisColumns)
                    {                        
                        var chartAxis = new ChartAxis() { Name = axisColumn, Values = new object[dataTable.Rows.Count] };                        
                        chartAxisGroup.AxisList.Add(chartAxis);
                        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                        {
                            chartAxis.Values[rowIndex] = Convert.ToSingle(dataTable.Rows[rowIndex][axisColumn]);
                        }                        
                    }
                    chartData.AxisGroups.Add(chartAxisGroup);
                }

                return chartData;
            }
            catch(Exception exception)
            {                
                throw;
            }
        }
    }
}
