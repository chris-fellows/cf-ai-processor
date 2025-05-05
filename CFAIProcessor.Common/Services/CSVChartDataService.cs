using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFCSV.Reader;

namespace CFAIProcessor.Services
{
    /// <summary>
    /// Chart data from CSV
    /// </summary>
    public class CSVChartDataService : IChartDataService
    {
        public ChartData GetChartData(string chartTypeId, ChartConfig chartConfig)
        {            
            try
            {                
                if (!File.Exists(chartConfig.DataSetInfo.DataSource))
                {
                    throw new FileNotFoundException("CSV file does not exist", chartConfig.DataSetInfo.DataSource);
                }

                // Set CSV reader for returning rows as Dictionary
                var csvReader = new CSVDictionaryReader();                

                // Read CSV settings (Columns, delimiter etc)
                var csvSettings = csvReader.GetSettings(chartConfig.DataSetInfo.DataSource);

                // Configure reader
                csvReader.File = chartConfig.DataSetInfo.DataSource;
                csvReader.Delimiter = csvSettings.Delimiter;
                csvReader.Encoding = csvSettings.Encoding;
                foreach(var column in csvSettings.Columns)
                {
                    if (chartConfig.AxisGroups.SelectMany(ag => ag.AxisColumns).Contains(column.Name))
                    {
                        csvReader.AddPropertyMapping<string>(column.Name, (headers, values) => values[headers.IndexOf(column.Name)]);
                    }
                }
                
                // Set function for creating empty CSV row. Only need columns that we'll use
                var createRowFunction = () =>
                {
                    var row = new Dictionary<string, object>();
                    foreach(var column in chartConfig.AxisGroups.SelectMany(ag => ag.AxisColumns).Distinct())
                    {
                        row.Add(column, null);
                    } 
                    return row;
                };

                // Load data             
                //var dataTable = new CSVDataTableReader().Read(chartConfig.DataSetInfo.DataSource);                

                var chartData = new ChartData();

                // Set list of axis groups
                foreach (var chartConfigAxisGroup in chartConfig.AxisGroups)
                {
                    // Create axis group
                    var chartAxisGroup = new ChartAxisGroup()
                    {
                        Name = $"Group {chartConfig.AxisGroups.IndexOf(chartConfigAxisGroup) + 1}",
                        Mode = chartConfigAxisGroup.Mode,
                        Color = chartConfigAxisGroup.Color,
                    };
                    
                    // Add axis columns to group
                    foreach (var axisColumn in chartConfigAxisGroup.AxisColumns)
                    {
                        var chartAxis = new ChartAxis() { Name = axisColumn, Values = new object[0] };
                        chartAxisGroup.AxisList.Add(chartAxis);
                    }

                    chartData.AxisGroups.Add(chartAxisGroup);
                }

                // Get CSV axis values by column name
                var allChartConfigAxisColumns = chartConfig.AxisGroups.SelectMany(ag => ag.AxisColumns).Distinct().ToList();
                var valuesByColumnName = new Dictionary<string, List<object>>();               
                foreach(var column in allChartConfigAxisColumns)
                {
                    valuesByColumnName.Add(column, new());                
                }
                foreach (var csvRow in csvReader.Read(() => createRowFunction(), null))
                {
                    foreach(var chartConfigAxisColumn in allChartConfigAxisColumns)
                    {
                        var columnValue = csvRow[chartConfigAxisColumn];
                        valuesByColumnName[chartConfigAxisColumn].Add(columnValue);                       
                    }                                 
                }

                // Set axis values
                foreach(var chartAxisGroup in chartData.AxisGroups)
                {
                    foreach(var chartAxis in chartAxisGroup.AxisList)
                    {
                        chartAxis.Values = valuesByColumnName[chartAxis.Name].ToArray();                        
                    }

                    // Sort axis values on the X axis. This is necessary for line charts so that the line travels from left to right instead
                    // of zig-zagging.
                    if (chartAxisGroup.AxisList.Any())
                    {
                        chartAxisGroup.SortAxisValues(0);
                    }
                }

                //foreach (var chartConfigAxisGroup in chartConfig.AxisGroups)
                //{
                //    // Create axis group
                //    var chartAxisGroup = new ChartAxisGroup()
                //    {
                //        Name = $"Group {chartConfig.AxisGroups.IndexOf(chartConfigAxisGroup) + 1}",
                //        Mode = chartConfigAxisGroup.Mode,
                //        Color = chartConfigAxisGroup.Color,
                //    };
                 
                //    foreach(var row in csvReader.Read(() => createRowFunction(), null))
                //    {
                        
                //    }

                //    foreach (var axisColumn in chartConfigAxisGroup.AxisColumns)
                //    {
                //         me = axisColumn, Values = new object[dataTable.Rows.Count] };                        
                //        chartAxisGroup.AxisList.Add(chartAxis);
                //        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                //        {
                //            chartAxis.Values[rowIndex] = Convert.ToSingle(dataTable.Rows[rowIndex][axisColumn]);
                //        }                        
                //    }
                //    chartData.AxisGroups.Add(chartAxisGroup);

                //    // Sort axis values based on the X axis. This is necessary for line charts otherwise the line zigzags instead
                //    // of travelling from left to right.
                //    if (chartAxisGroup.AxisList.Any())
                //    {
                //        chartAxisGroup.SortAxisValues(0);
                //    }
                //}

                return chartData;
            }
            catch(Exception exception)
            {                
                throw;
            }
        }
    }
}
