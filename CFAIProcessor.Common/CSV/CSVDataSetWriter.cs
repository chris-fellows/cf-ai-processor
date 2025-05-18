using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using CFCSV.Writer;
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
        private readonly Encoding _encoding;

        public CSVDataSetWriter(string file, Char delimiter, Encoding encoding)
        {
            _file = file;
            _delimiter = delimiter;
            _encoding = encoding;
        }
   
        public void WriteRow(Dictionary<string, string> row)
        {
            var isWriteFileHeaders = !File.Exists(_file);

            var csvWriter = new CSVDictionaryWriter()
            {
                File = _file,
                Delimiter = _delimiter,
                Encoding = _encoding
            };
            var rowNew = new Dictionary<string, object>();
            foreach (var column in row.Keys)
            {
                csvWriter.AddColumn(column, (row) => row[column].ToString());
                rowNew.Add(column, row[column]);
            }
            csvWriter.Write(new[] { rowNew });

            // Create dummy CSV config so that it matches other CSVs
            var file = Path.Combine(Path.GetDirectoryName(_file), $"{Path.GetFileNameWithoutExtension(_file)}.json");
            CreateCSVConfig(file, row);

            //using (var streamWriter = new StreamWriter(_file, true, Encoding.UTF8))
            //{
            //    var line = new StringBuilder("");

            //    // Write headers
            //    if (isWriteFileHeaders)
            //    {
            //        foreach (var column in row.Keys)
            //        {
            //            if (line.Length > 0) line.Append(_delimiter);
            //            line.Append(column);
            //        }
            //        streamWriter.WriteLine(line.ToString());
            //        line.Length = 0;

            //        // Create dummy CSV config so that it matches other CSVs
            //        var file = Path.Combine(Path.GetDirectoryName(_file), $"{Path.GetFileNameWithoutExtension(_file)}.json");
            //        CreateCSVConfig(file, row);
            //    }

            //    // Write row
            //    foreach (var column in row.Keys)
            //    {
            //        if (line.Length > 0) line.Append(_delimiter);
            //        line.Append(row[column]);
            //    }
            //    streamWriter.WriteLine(line.ToString());
            //}
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
