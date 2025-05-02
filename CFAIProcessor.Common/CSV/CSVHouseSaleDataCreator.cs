using CFAIProcessor.Constants;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;

namespace CFAIProcessor.CSV
{
    /// <summary>
    /// Creates CSV house sale data using random data
    /// </summary>
    public class CSVHouseSaleDataCreator : ICSVDataCreator
    {
        private List<CSVRowGroup> _rowGroups = new();

        public void Create(string dataFile, string configFile, char delimiter, int maxRecords, string[]? columnNames)
        {
            _rowGroups = GetRowGroups();

            var random = new Random();            

            // Set config
            var config = new CSVDataConfig<HouseSaleData>()
            {
                ConfigFile = configFile,   
                DataFile = dataFile,
                Delimiter = delimiter,
                MaxRecords = maxRecords,
                Columns = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        ExternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        IsFeature = true,
                        IsLabel = false,
                        MinValue = 1,
                        MaxValue = 10
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        ExternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        IsFeature = true,
                        IsLabel = false,
                        MinValue = 1,
                        MaxValue = 100000                        
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        ExternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        IsFeature = false,
                        IsLabel = true,
                        MinValue = 1,
                        MaxValue = 2000000                        
                    }
                },                
                CreateRandomEntity = () =>
                {
                    return CreateRandomEntity(random, _rowGroups);
                },
                GetColumnValues = (houseSaleData) =>
                {
                    var values = new List<string>();
                    if (columnNames == null || columnNames.Length == 0 || columnNames.Contains(CSVHouseSaleDataColumnNames.NumberOfBeds))
                    {
                        values.Add(houseSaleData.NumberOfBeds.ToString());
                    }
                    if (columnNames == null || columnNames.Length == 0 || columnNames.Contains(CSVHouseSaleDataColumnNames.SizeInSquareFeet))
                    {
                        values.Add(houseSaleData.SizeInSquareFeet.ToString());
                    }
                    if (columnNames == null || columnNames.Length == 0 || columnNames.Contains(CSVHouseSaleDataColumnNames.SalePrice))
                    {
                        values.Add(houseSaleData.SalePrice.ToString());
                    }
                    return values.ToArray();                   
                }
            };
            if (columnNames != null && columnNames.Length > 0)
            {
                config.Columns.RemoveAll(c => !columnNames.Contains(c.InternalName));                
            }

            // Create data
            var creator = new CSVRandomDataCreator();
            creator.Create(config);
        }

        private static HouseSaleData CreateRandomEntity(Random random, List<CSVRowGroup> rowGroups)
        {
            // Get random row group
            var rowGroup = rowGroups[random.Next(0, rowGroups.Count - 1)];

            return new HouseSaleData()
            {
                NumberOfBeds = rowGroup.ColumnConfig(CSVHouseSaleDataColumnNames.NumberOfBeds).GetRandomValue(random),
                SizeInSquareFeet = NumericUtilities.RoundDownToNearestDivisor(rowGroup.ColumnConfig(CSVHouseSaleDataColumnNames.SizeInSquareFeet).GetRandomValue(random), 1000),
                SalePrice = NumericUtilities.RoundDownToNearestDivisor(rowGroup.ColumnConfig(CSVHouseSaleDataColumnNames.SalePrice).GetRandomValue(random), 10000),
            };
        }

        private List<CSVRowGroup> GetRowGroups()
        {
            var rowGroups = new List<CSVRowGroup>();

            rowGroups.Add(new CSVRowGroup()
            {
                Name = "1 bed",
                ColumnConfigs = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 1,
                        MaxValue = 1,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 1000,
                        MaxValue = 5000
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 100000,
                        MaxValue = 200000
                    },
                }
            });

            rowGroups.Add(new CSVRowGroup()
            {
                Name = "2 bed",
                ColumnConfigs = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 2,
                        MaxValue = 2,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 5000,
                        MaxValue = 9000
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 200000,
                        MaxValue = 300000
                    },
                }
            });

            rowGroups.Add(new CSVRowGroup()
            {
                Name = "3 bed",
                ColumnConfigs = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 3,
                        MaxValue = 3,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 10000,
                        MaxValue = 20000
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 300000,
                        MaxValue = 450000
                    },
                }
            });

            rowGroups.Add(new CSVRowGroup()
            {
                Name = "4 bed",
                ColumnConfigs = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 4,
                        MaxValue = 4,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 20000,
                        MaxValue = 30000,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 450000,
                        MaxValue = 600000
                    },
                }
            });

            rowGroups.Add(new CSVRowGroup()
            {
                Name = "5 bed",
                ColumnConfigs = new List<CSVColumnConfig>()
                {
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 5,
                        MaxValue = 5,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 30000,
                        MaxValue = 40000,
                    },
                    new CSVColumnConfig()
                    {
                        InternalName = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 600000,
                        MaxValue = 800000
                    },
                }
            });

            return rowGroups;
        }  
    }
}
