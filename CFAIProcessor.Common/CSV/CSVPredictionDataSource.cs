using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System.Data;
using Tensorflow.NumPy;

namespace CFAIProcessor.CSV
{
    /// <summary>
    /// CSV data source for predictions
    /// </summary>
    internal class CSVPredictionDataSource : IPredictionDataSource
    {
        private string _dataFile { get; set; } = string.Empty;        
        private readonly CSVConf _csvConfig;

        public CSVPredictionDataSource(string dataFile, string  configFile)
        {
            _dataFile = dataFile;           

            if (!File.Exists(_dataFile))
            {
                throw new FileNotFoundException("Data file does not exist", _dataFile);
            }

            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException("Config file does not exist", configFile);
            }

            _csvConfig = JsonUtilities.DeserializeFromString<CSVConf>(File.ReadAllText(configFile), JsonUtilities.DefaultJsonSerializerOptions);
        }

        public NDArray GetFeatureValues(bool normalise)
        {
            var dataTable = GetDataAsDataTable(_dataFile);
            
            var featureColumns = _csvConfig.Columns.Where(c => c.IsFeature).ToList();

            var features = np.zeros((dataTable.Rows.Count, featureColumns.Count), Tensorflow.TF_DataType.TF_FLOAT);

            //var features = np.zeros((items.Count(), 2), Tensorflow.TF_DataType.TF_FLOAT);
            
            for (int rowIndex =0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var rowValues = new float[featureColumns.Count];

                for(int columnIndex = 0; columnIndex < featureColumns.Count; columnIndex++)
                {
                    var featureColumn = featureColumns[columnIndex];
                    if (normalise)
                    {
                        var value = Convert.ToSingle(dataTable.Rows[rowIndex][featureColumn.InternalName]);
                        rowValues[columnIndex] = Convert.ToSingle(NumericUtilities.Normalize(value, featureColumn.MinValue, featureColumn.MaxValue, 0, 1));
                    }
                    else
                    {
                        rowValues[columnIndex] = Convert.ToSingle(dataTable.Rows[rowIndex][featureColumn.InternalName]);
                    }
                }

                features[rowIndex] = rowValues;                
            }

            return features;
        }

        public string[] FeatureNames => _csvConfig.Columns.Where(c => c.IsFeature).Select(c => c.InternalName).ToArray();

        public NDArray GetLabelValues(bool normalise)
        {
            var dataTable = GetDataAsDataTable(_dataFile);
            
            var labelColumns = _csvConfig.Columns.Where(c => c.IsLabel).ToList();

            var labels = np.zeros((dataTable.Rows.Count, labelColumns.Count), Tensorflow.TF_DataType.TF_FLOAT);

            //var labels = np.zeros((items.Count(), 1), Tensorflow.TF_DataType.TF_FLOAT);

            //var minSalePrice = 1;
            //var maxSalePrice = 10000000;

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var rowValues = new float[labelColumns.Count];               

                for (int columnIndex = 0; columnIndex < labelColumns.Count; columnIndex++)
                {
                    var labelColumn = labelColumns[columnIndex];
                    if (normalise)
                    {
                        var value = Convert.ToSingle(dataTable.Rows[rowIndex][labelColumn.InternalName]);
                        rowValues[columnIndex] = Convert.ToSingle(NumericUtilities.Normalize(value, labelColumn.MinValue, labelColumn.MaxValue, 0, 1));
                    }
                    else
                    {
                        rowValues[columnIndex] = Convert.ToSingle(dataTable.Rows[rowIndex][labelColumn.InternalName]);
                    }
                }

                labels[rowIndex] = rowValues;

                /*
                rowIndex++;
                features[rowIndex] = new[]
                {
                    normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.NumberOfBeds, minNumberOfBeds, maxNumberOfBeds, 0, 1)) :
                            Convert.ToSingle(item.NumberOfBeds),
                    normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.SizeInSquareFeet, minSquareFeet, maxSquareFeet, 0, 1)) :
                            Convert.ToSingle(item.SizeInSquareFeet)
                };
                */
            }

            //int rowIndex = -1;
            //foreach (var item in items)
            //{
            //    rowIndex++;
            //    labels[rowIndex] = new[]
            //    {
            //        normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.SalePrice, minSalePrice, maxSalePrice, 0, 1)) :
            //            Convert.ToSingle(item.SalePrice)
            //    };
            //}

            return labels;
        }

        public string[] LabelNames => _csvConfig.Columns.Where(c => c.IsLabel).Select(c => c.InternalName).ToArray();    

        private DataTable GetDataAsDataTable(string dataFile)
        {
            if (!File.Exists(dataFile))
            {
                throw new FileNotFoundException("Data file does not exist", dataFile);
            }
            
            var dataTable = new DataTable();            
            foreach(var column in _csvConfig.Columns)
            {
                dataTable.Columns.Add(column.InternalName, typeof(string));
            }
      
            using (var reader = new StreamReader(dataFile))
            {
                int lineCount = 0;
                var headers = new List<string>();
                while (!reader.EndOfStream)
                {
                    lineCount++;
                    var line = reader.ReadLine();
                    var elements = line.Split('\t');
                    if (lineCount == 1)
                    {
                        headers = elements.ToList();
                    }
                    else
                    {
                        var row = dataTable.NewRow();

                        for(int index = 0; index < elements.Length; index++)
                        {
                            var columnName = headers[index];
                            var columnValue = elements[index];

                            var columnConfig = _csvConfig.Columns.FirstOrDefault(c=> c.InternalName.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                            if (columnConfig != null)
                            {
                                row[columnName] = columnValue;
                            }                            
                        }

                        dataTable.Rows.Add(row);

                        //var item = new HouseSaleData()
                        //{
                        //    NumberOfBeds = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.NumberOfBeds)]),
                        //    SizeInSquareFeet = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SizeInSquareFeet)]),
                        //    SalePrice = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SalePrice)]),
                        //};
                        //items.Add(item);
                    }
                }
            }
            
            return dataTable;
        }

        //private List<HouseSaleData> GetData(string dataFile)
        //{
        //    if (!File.Exists(dataFile))
        //    {
        //        throw new FileNotFoundException("Data file does not exist", dataFile);
        //    }

        //    var items = new List<HouseSaleData>();

        //    using (var reader = new StreamReader(dataFile))
        //    {
        //        int lineCount = 0;
        //        var headers = new List<string>();
        //        while (!reader.EndOfStream)
        //        {
        //            lineCount++;
        //            var line = reader.ReadLine();
        //            var elements = line.Split('\t');
        //            if (lineCount == 1)
        //            {
        //                headers = elements.ToList();
        //            }
        //            else
        //            {
        //                var item = new HouseSaleData()
        //                {
        //                    NumberOfBeds = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.NumberOfBeds)]),
        //                    SizeInSquareFeet = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SizeInSquareFeet)]),
        //                    SalePrice = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SalePrice)]),
        //                };
        //                items.Add(item);
        //            }
        //        }
        //    }

        //    return items;
        //}

        public float GetNonNormalisedFeatureValue(int columnIndex, float value)
        {
            var featureColumns = _csvConfig.Columns.Where(c => c.IsFeature).ToList();
            var column = featureColumns[columnIndex];
            return Convert.ToSingle(NumericUtilities.Normalize(value, 0, 1, column.MinValue, column.MaxValue));
        }

        public float GetNonNormalisedLabelValue(int columnIndex, float value)
        {
            var labelColumns = _csvConfig.Columns.Where(c => c.IsLabel).ToList();
            var column = labelColumns[columnIndex];
            return Convert.ToSingle(NumericUtilities.Normalize(value, 0, 1, column.MinValue, column.MaxValue));
        }
    }
}
