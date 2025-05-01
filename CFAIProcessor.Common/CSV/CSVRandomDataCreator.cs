using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.CSV
{
    internal class CSVRandomDataCreator
    {
        public void Create<TEntityType>(CSVDataConfig<TEntityType> config)
        {
            if (File.Exists(config.File))
            {
                File.Delete(config.File);
            }

            using (var streamWriter = new StreamWriter(config.File))
            {
                var line = new StringBuilder("");

                // Write columns
                foreach(var columnName in config.ColumnNames)
                {
                    if (line.Length > 0) line.Append(config.Delimiter);
                    line.Append(columnName);
                }
                streamWriter.WriteLine(line.ToString());

                // Write rows
                line.Length = 0;
                for (int index = 0; index < config.MaxRecords; index++)
                {                    
                    var entity = config.CreateRandomEntity();

                    var columnValues = config.GetColumnValues(entity);
                    
                    foreach(var columnValue in columnValues)
                    {
                        if (line.Length > 0) line.Append(config.Delimiter);

                        line.Append(columnValue);
                    }
                    streamWriter.WriteLine(line.ToString());
                    line.Length = 0;
                }
            }
        }
    }
}
