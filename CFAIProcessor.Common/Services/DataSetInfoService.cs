using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;

namespace CFAIProcessor.Services
{
    public class DataSetInfoService : IDataSetInfoService
    {
        private readonly string _folder;

        public DataSetInfoService(string folder)
        {
            _folder = folder;
        }

        public List<DataSetInfo> GetAll()
        {
            var list = new List<DataSetInfo>();

            foreach (var file in Directory.GetFiles(_folder, "*.txt"))
            {
                // Load CSV config file
                var configFile = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}.json");
                var csvConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(configFile), JsonUtilities.DefaultJsonSerializerOptions);

                // Add dataset info
                var dataSetInfo = new DataSetInfo()
                {
                    Id = Path.GetFileName(file),
                    Name = Path.GetFileNameWithoutExtension(file),
                    Columns = csvConfig.Columns.Select(column =>
                    {
                        return new DataSetColumn()
                        {
                            InternalName = column.InternalName,
                            ExternalName = column.ExternalName
                        };
                    }).ToList(),
                    DataSource = file,                                       
                };

                list.Add(dataSetInfo);
            }

            return list.OrderBy(ds => ds.Name).ToList();            
        }

        public DataSetInfo? GetById(string id)
        {
            var file = Path.Combine(_folder, id);
            if (File.Exists(file))
            {
                // Load CSV config file
                var configFile = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}.json");
                var csvConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(configFile), JsonUtilities.DefaultJsonSerializerOptions);

                // Add dataset info
                var dataSetInfo = new DataSetInfo()
                {
                    Id = Path.GetFileName(file),
                    Name = Path.GetFileNameWithoutExtension(file),
                    Columns = csvConfig.Columns.Select(column =>
                    {
                        return new DataSetColumn()
                        {
                            InternalName = column.InternalName,
                            ExternalName = column.ExternalName
                        };
                    }).ToList(),
                    DataSource = file,
                };

                return dataSetInfo;
            }

            return null;
        }

        public void Add(DataSetInfo dataSetInfo, string tempFile)
        {
            if (!String.IsNullOrEmpty(tempFile))
            {
                File.Move(tempFile, dataSetInfo.DataSource);
            }
        }
    }
}
