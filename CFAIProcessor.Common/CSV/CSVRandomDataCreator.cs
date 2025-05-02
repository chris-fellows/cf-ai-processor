using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.CSV
{
    internal class CSVRandomDataCreator
    {
        public void Create<TEntityType>(CSVDataConfig<TEntityType> config)
        {
            if (File.Exists(config.DataFile))
            {
                File.Delete(config.DataFile);
            }
            if (File.Exists(config.ConfigFile))
            {
                File.Delete(config.ConfigFile);
            }

            using (var streamWriter = new StreamWriter(config.DataFile))
            {
                var line = new StringBuilder("");

                var csvConfig = new CSVConfig() { Columns = new() };

                // Write columns
                foreach(var column in config.Columns)
                {
                    if (line.Length > 0) line.Append(config.Delimiter);
                    line.Append(column.InternalName);

                    csvConfig.Columns.Add(column);
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

                // Save config
                File.WriteAllText(config.ConfigFile, JsonUtilities.SerializeToString(csvConfig, JsonUtilities.DefaultJsonSerializerOptions));
            }
        }
    }
}
