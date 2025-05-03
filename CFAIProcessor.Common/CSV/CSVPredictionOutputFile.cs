using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.CSV
{
    /// <summary>
    /// Prediction output file in CSV format
    /// </summary>
    public class CSVPredictionOutputFile : IPredictionOutputFile, IDisposable
    {
        private string _dataFile { get; set; } = string.Empty;
        private readonly CSVConfig _csvTestConfig;      // Config for test file
        private readonly Char _delimiter = (Char)9;
        private StreamWriter? _writer = null;

        public CSVPredictionOutputFile(string dataFile, CSVConfig csvTestConfig)
        {
            _dataFile = dataFile;
            _csvTestConfig = csvTestConfig;
            
            if (File.Exists(_dataFile))
            {
                File.Delete(_dataFile);
            }
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer.Dispose();
            }
        }

        public void Write(float[] features, float[] labels, float[] labelsPredicted)
        {
            // Get feature & label columns
            var featureColumns = _csvTestConfig.Columns.Where(c => c.IsFeature).ToList();
            var labelColumns = _csvTestConfig.Columns.Where(c => c.IsLabel).ToList();

            var line = new StringBuilder("");

            // Open file if required
            if (_writer == null)    
            {
                _writer = new StreamWriter(_dataFile, true);

                // Write header                
                foreach(var column in _csvTestConfig.Columns)
                {
                    if (line.Length > 0) line.Append(_delimiter);
                    line.Append(column.InternalName);

                    if (column.IsLabel)
                    {                        
                        line.Append($"{_delimiter}{column.InternalName}_predicted");
                    }                    
                }

                // If original file contained no labels then add 
                if (!labels.Any() && labelsPredicted.Any())
                {
                    for(int index =0; index < labelsPredicted.Length; index++)
                    {
                        if (line.Length > 0) line.Append(_delimiter);
                        line.Append($"predicted_{index + 1}");
                    }
                }

                _writer.WriteLine(line.ToString());
                line.Length = 0;
            }

            // Write row
            // If the test file contained labels then we'll write columns as [column_name_1],[predicted_column_name_1]
            foreach (var column in _csvTestConfig.Columns)
            {                              
                if (column.IsFeature)
                {
                    if (line.Length > 0) line.Append(_delimiter);

                    var featureIndex = featureColumns.IndexOf(column);
                    line.Append(features[featureIndex].ToString());
                }
                else if (column.IsLabel)
                {
                    if (line.Length > 0) line.Append(_delimiter);

                    // Add original label
                    var labelIndex = labelColumns.IndexOf(column);
                    line.Append(labels[labelIndex].ToString());
                    
                    // Add predicted label
                    line.Append($"{_delimiter}{labelsPredicted[labelIndex].ToString()}");
                }               
            }

            // If original file contained no labels then add labels. We don't don't the column named so just create a default name
            if (!labels.Any() && labelsPredicted.Any())
            {
                for (int index = 0; index < labelsPredicted.Length; index++)
                {
                    if (line.Length > 0) line.Append(_delimiter);
                    line.Append($"predicted_{index + 1}");
                }
            }

            _writer.WriteLine(line.ToString());
            _writer.Flush();
        }
    }
}
