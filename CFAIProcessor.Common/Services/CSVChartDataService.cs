using CFAIProcessor.Common.Models;
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
            var cp = "CP100";
            try
            {
                // Load data
                cp = "CP200";
                var dataTable = new CSVDataTableReader().Read(chartConfig.DataSetInfo.DataSource);
                cp = "CP200";

                var chartData = new ChartData();

                // Add single axis group
                var chartAxisGroup = new ChartAxisGroup();
                foreach (var column in chartConfig.AxisColumns)
                {
                    cp = "CP300";
                    var chartAxis = new ChartAxis() { Name = column, Values = new float[dataTable.Rows.Count] };
                    cp = "CP400";
                    chartAxisGroup.AxisList.Add(chartAxis);
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        chartAxis.Values[rowIndex] = Convert.ToSingle(dataTable.Rows[rowIndex][column]);
                    }
                    cp = "CP500";
                }
                chartData.AxisGroups.Add(chartAxisGroup);

                return chartData;
            }
            catch(Exception exception)
            {
                var newCP = cp;
                throw;
            }
        }
    }
}
