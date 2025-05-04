using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.CSV
{
    internal class CSVDataSetWriter : IDataSetWriter
    {
        private readonly string _file;
        private readonly Char _delimiter;

        public CSVDataSetWriter(string file, Char delimiter)
        {
            _file = file;
            _delimiter = delimiter;
        }
   
        public void WriteRow(Dictionary<string, string> row)
        {
            var isFileHeaders = !File.Exists(_file);

           

            using (var streamWriter = new StreamWriter(_file, true, Encoding.UTF8))
            {
                var line = new StringBuilder("");

                // Write headers
                if (isFileHeaders)
                {
                    foreach (var column in row.Keys)
                    {
                        if (line.Length > 0) line.Append(_delimiter);
                        line.Append(column);
                    }
                    streamWriter.WriteLine(line.ToString());
                    line.Length = 0;

                    // Create dummy CSV config so that it matches other CSVs
                    var file = Path.Combine(Path.GetDirectoryName(_file), $"{Path.GetFileNameWithoutExtension(_file)}.json");
                    CreateCSVConfig(file, row);
                }

                // Write row
                foreach (var column in row.Keys)
                {
                    if (line.Length > 0) line.Append(_delimiter);
                    line.Append(row[column]);
                }
                streamWriter.WriteLine(line.ToString());
            }
        }

        private void CreateCSVConfig(string file, Dictionary<string, string> row)
        {
            var csvConfig = new CSVConfig()
            {                
                Columns = row.Keys.Select(column =>
                {
                    return new CSVColumnConfig()
                    {
                        InternalName = column,
                        ExternalName = column
                    };
                }).ToList()
            };
            File.WriteAllText(file, JsonUtilities.SerializeToString(csvConfig, JsonUtilities.DefaultJsonSerializerOptions));
        }

    }
}
