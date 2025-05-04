using CFAIProcessor.Enums;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class AggregateService : IAggregateService
    {            
        public DataTable Aggregate(DataTable dataTable, AggregateConfig  aggregateConfig)
        {

            var dataTableDest = new DataTable();

            // Add columns
            foreach (var aggregateColumn in aggregateConfig.Columns)
            {
                dataTableDest.Columns.Add(aggregateColumn.OutputName, typeof(string));
            }

            // Get columns that just need to be copied
            var copyColumns = aggregateConfig.Columns.Where(c => c.AggregateAction == AggregateActions.None).ToList();

            // Get columns that need aggregating
            var groupByColumns = aggregateConfig.Columns.Where(c => c.GroupByColumnInternalNames.Any());

            // We work based on creating rows for columns that just need unaggregated value to be copied and then we process the aggregations
            // for these rows. If we don't have any unaggregated values to copy then we can't do the aggregated columns.
            if (!copyColumns.Any())
            {
                throw new ArgumentException("Unable to aggregate unless there are columns to copy");
            }

            // Process each aggregage column with a group by:
            // 1) Get all column values for the grouping.
            // 2) Loop through each distinct set of column values and aggregate.
            foreach(var groupByColumn in groupByColumns)
            {
                // Get all column values
                var allColumnValues = GetGroupByColumnValues(dataTable, groupByColumn);

                // Set dest data table column names to check when checking if we need to add a new row
                var destColumnNames = new List<string>();
                foreach (var columnName in groupByColumn.GroupByColumnInternalNames)
                {
                    var copyColumn = copyColumns.FirstOrDefault(c => c.InputName == columnName);
                    if (copyColumn == null)    // Error
                    {
                        // Column is not specified for destination. We need the column to exist.
                        int zzzz = 1000;
                    }
                    else
                    {
                        destColumnNames.Add(copyColumn.OutputName);
                    }
                }

                // For each set of column values then aggregate
                for (int index = 0; index < allColumnValues.Count; index++)
                {
                    // Get rows
                    var rows = GetRowsByColumnValues(dataTable, groupByColumn.GroupByColumnInternalNames, allColumnValues[index].ToList());

                    var aggregateValue = GetAggregateValue(rows, groupByColumn.InputName, groupByColumn.AggregateAction, groupByColumn.DecimalPlaces);               

                    var rowsDest = GetRowsByColumnValues(dataTableDest,
                                                    destColumnNames,
                                                    allColumnValues[index].ToList());
                    if (rowsDest.Length == 0)       // Add row
                    {
                        var row = dataTableDest.NewRow();

                        // Set column values for aggregated value
                        for(int myIndex = 0; myIndex < allColumnValues[index].Length; myIndex++)
                        {
                            row[destColumnNames[myIndex]] = allColumnValues[index][myIndex];
                        }

                        // Set other columns that get copied and are converted from other values
                        foreach(var copyColumn in copyColumns)
                        {
                            if (destColumnNames.Contains(copyColumn.InputName) &&
                                !destColumnNames.Contains(copyColumn.OutputName) &&
                                copyColumn.NumberConvertAction != null)
                            {
                                row[copyColumn.OutputName] = GetSourceColumnValue(allColumnValues[index][destColumnNames.IndexOf(copyColumn.InputName)] , copyColumn);
                                int xxxxxxx = 1000;
                            }
                        }

                        // Set aggregated value
                        row[groupByColumn.OutputName] = aggregateValue;

                        dataTableDest.Rows.Add(row);
                    }
                    else    // Update row (Probably added for another aggregate function)
                    {
                        rowsDest[0][groupByColumn.OutputName] = aggregateValue;
                    }
                }
            }

            return dataTableDest;
        }        

        //public DataTable Aggregate(DataTable dataTable, AggregateConfig aggregateConfig)
        //{
        //    var dataTableDest = new DataTable();

        //    // Add columns
        //    foreach(var aggregateColumn in aggregateConfig.Columns)
        //    {
        //        dataTableDest.Columns.Add(aggregateColumn.OutputName, typeof(string));                
        //    }

        //    // Get columns that just need to be copied
        //    var copyColumns = aggregateConfig.Columns.Where(c => c.AggregateAction == AggregateActions.None).ToList();

        //    // Get columns that need aggregating
        //    var groupByColumns = aggregateConfig.Columns.Where(c => c.GroupByColumnInternalNames.Any());

        //    // We work based on creating rows for columns that just need unaggregated value to be copied and then we process the aggregations
        //    // for these rows. If we don't have any unaggregated values to copy then we can't do the aggregated columns.
        //    if (!copyColumns.Any())
        //    {
        //        throw new ArgumentException("Unable to aggregate unless there are columns to copy");
        //    }

        //    // For columns that just need to be copied & converted (E.g. Modulo rounding) then add a temporary column to source data table
        //    if (copyColumns.Any(c => c.NumberConvertAction != null))
        //    {
        //        foreach (var copyColumn in copyColumns.Where(c => c.NumberConvertAction != null))
        //        {
        //            dataTable.Columns.Add(copyColumn.OutputName, typeof(String));
        //        }

        //        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        //        {
        //            foreach (var copyColumn in copyColumns)
        //            {
        //                dataTable.Rows[rowIndex][copyColumn.OutputName] = GetSourceColumnValue(dataTable.Rows[rowIndex][copyColumn.InputName], copyColumn);
        //                //var value = dataTable.Rows[rowIndex][copyColumn.OutputName];
        //            }
        //        }
        //    }

        //    // Copy columns that need no aggregation
        //    for (int sourceRowIndex = 0;  sourceRowIndex < dataTable.Rows.Count; sourceRowIndex++)
        //    {
        //        // Copy column values to copy.
        //        // Some values may be converted. E.g. Use modulo to round values
        //        var sourceColumnValues = new List<object>();
        //        foreach (var copyColumn in copyColumns)
        //        {                    
        //            var sourceColumnValue = GetSourceColumnValue(dataTable.Rows[sourceRowIndex][copyColumn.InputName], copyColumn);
        //            sourceColumnValues.Add(sourceColumnValue);
        //        }

        //        // Check if dest table contains values                
        //        var destRows = GetRowsByColumnValues(dataTableDest, copyColumns.Select(c => c.OutputName).ToList(),
        //                                sourceColumnValues);

        //        // Add rows
        //        if (!destRows.Any())
        //        {             
        //            var row = dataTableDest.NewRow();
        //            foreach (var copyColumn in copyColumns)
        //            {
        //                row[copyColumn.OutputName] = sourceColumnValues[copyColumns.IndexOf(copyColumn)];
        //            }
        //            dataTableDest.Rows.Add(row);
        //        }
        //    }

        //    // Add aggregate column values
        //    for (int destRowIndex = 0; destRowIndex < dataTableDest.Rows.Count; destRowIndex++)
        //    {
        //        // Copy column values to copy   
        //        var sourceColumnValues = new List<object>();
        //        foreach (var copyColumn in copyColumns)
        //        {                                            
        //            var sourceColumnValue = dataTableDest.Rows[destRowIndex][copyColumn.OutputName];
        //            sourceColumnValues.Add(sourceColumnValue);                    
        //        }

        //        // Get source rows. If we have a column that needs to be converted (E.g. Modulo rounded) then we pass
        //        // in the name of the temporary column (OutputName) that was created above on source table.
        //        var sourceRows = GetRowsByColumnValues(dataTable,                                                       
        //                                                copyColumns.Select(c => c.NumberConvertAction == null ?
        //                                                            c.InputName :
        //                                                            c.OutputName).ToList(),                     
        //                                                sourceColumnValues);

        //        // Copy aggregated value
        //        foreach(var groupColumn in groupByColumns)
        //        {
        //            var aggregateValue = GetAggregateValue(sourceRows,
        //                        groupColumn.InputName,
        //                        groupColumn.AggregateAction,
        //                        groupColumn.DecimalPlaces);                                

        //            dataTableDest.Rows[destRowIndex][groupColumn.OutputName] = aggregateValue;                    
        //        }
        //    }
                                              
        //    return dataTableDest;
        //}

        private static List<object[]> GetGroupByColumnValues(DataTable dataTable, AggregateColumn aggregateColumn)
        {
            var allColumnValues = new List<object[]>();

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                // For row then get column values for the group by columns
                var columnValues = new List<object>();
                foreach(var column in aggregateColumn.GroupByColumnInternalNames)
                {
                    var columnValue = dataTable.Rows[rowIndex][column];
                    columnValues.Add(columnValue);
                }

                // Check if we've encountered these group column values
                var isAddColumnValues = true;
                for(int index1 = 0; index1 < allColumnValues.Count; index1++)
                {
                    var isFoundInAllColumnValues = true;    // Default
                    for(int index2 = 0; index2 < columnValues.Count; index2++)
                    {
                        if (allColumnValues[index1][index2].ToString() != columnValues[index2].ToString())
                        {                                                        
                            isFoundInAllColumnValues = false;
                            break;
                        }
                    }
                    if (isFoundInAllColumnValues) isAddColumnValues = false;
                }

                if (isAddColumnValues)
                {
                    allColumnValues.Add(columnValues.ToArray());
                }
            }

            return allColumnValues;
        }

        private static object GetSourceColumnValue(object rawValue, AggregateColumn aggregateColumn)
        {
            if (aggregateColumn.NumberConvertAction != null)
            {
                var value = Convert.ToDouble(rawValue);

                return aggregateColumn.NumberConvertAction switch
                {
                    NumberConvertActions.ModuloRoundDown => (int)(value / aggregateColumn.NumberConvertModuloValue.Value) * aggregateColumn.NumberConvertModuloValue.Value,
                    NumberConvertActions.ModuloRoundUp => ((int)(value / aggregateColumn.NumberConvertModuloValue.Value) + 1) * aggregateColumn.NumberConvertModuloValue.Value,
                    _ => value
                };
            }

            return rawValue;
        }

        /// <summary>
        /// Gets aggregated value.
        /// 
        /// Note than we don't use sourceGroupColumnNames due to the way that we firstly copy over raw values first.
        /// </summary>
        /// <param name="sourceRows"></param>
        /// <param name="sourceColumnName"></param>
        /// <param name="aggregateAction"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="sourceGroupColumnNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private double GetAggregateValue(DataRow[] sourceRows, 
                                        string sourceColumnName,
                                        AggregateActions aggregateAction,
                                        int? decimalPlaces)                                        
        {            
            var values = new Dictionary<AggregateActions, double>();
            values.Add(AggregateActions.Min, Double.MaxValue);
            values.Add(AggregateActions.Max, Double.MinValue);
            values.Add(AggregateActions.Avg, 0);
            values.Add(AggregateActions.RowCount, 0);

            foreach (var sourceRow in sourceRows)
            {
                var value = Convert.ToDouble(sourceRow[sourceColumnName]);

                values[AggregateActions.Avg] += value;
                values[AggregateActions.RowCount] += 1;

                if (value < values[AggregateActions.Min]) values[AggregateActions.Min] = value;

                if (value > values[AggregateActions.Max]) values[AggregateActions.Max] = value;

            }            

            if (decimalPlaces != null)
            {
                return aggregateAction switch
                {
                    AggregateActions.Avg => Double.Round(values[AggregateActions.Avg] / sourceRows.Length, decimalPlaces.Value),
                    AggregateActions.Max => Double.Round(values[AggregateActions.Max], decimalPlaces.Value),
                    AggregateActions.Min => Double.Round(values[AggregateActions.Min], decimalPlaces.Value),
                    AggregateActions.RowCount => values[AggregateActions.RowCount],
                    _ => throw new ArgumentException("Invalid aggregate action")    // None
                };
            }

            return aggregateAction switch
            {
                AggregateActions.Avg => values[AggregateActions.Avg] / sourceRows.Length,
                AggregateActions.Max => values[AggregateActions.Max],
                AggregateActions.Min => values[AggregateActions.Min],
                AggregateActions.RowCount => values[AggregateActions.RowCount],
                _ => throw new ArgumentException("Invalid aggregate action")    // None
            };
        }

        private static DataRow[] GetRowsByColumnValues(DataTable dataTable,                                                            
                                                         List<string> columnNames, List<object> columnValues)
        {
            var query = new StringBuilder("");
            for (int columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
            {
                if (columnIndex > 0) query.Append(" AND ");

                query.Append($"{columnNames[columnIndex]} = '{columnValues[columnIndex]}' ");
            }

            var rows = dataTable.Select(query.ToString());

            return rows;
        }

        private static List<object> GetDistinctValues(DataRow[] rows, string columnName)
        {
            var values = new List<object>();

            for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
            {
                var value = rows[rowIndex][columnName];
                if (value == null || value == DBNull.Value)
                {
                    value = null;
                }
                if (!values.Contains(value))
                {
                    values.Add(value);
                }
            }

            return values;
        }
    }
}
