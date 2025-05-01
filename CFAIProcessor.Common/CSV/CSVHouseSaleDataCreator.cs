using CFAIProcessor.Constants;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;

namespace CFAIProcessor.CSV
{
    /// <summary>
    /// Creates CSV house sale data using random data
    /// </summary>
    public class CSVHouseSaleDataCreator
    {
        private List<CSVRowGroup> _rowGroups = new();

        public void Create(string dataFile, char delimiter, int maxRecords)
        {
            _rowGroups = GetRowGroups();

            var random = new Random();            

            // Set config
            var config = new CSVDataConfig<HouseSaleData>()
            {
                File = dataFile,
                Delimiter = delimiter,
                MaxRecords = maxRecords,
                ColumnNames = new()
                {
                    CSVHouseSaleDataColumnNames.NumberOfBeds,
                    CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                    CSVHouseSaleDataColumnNames.SalePrice
                },
                CreateRandomEntity = () =>
                {
                    return CreateRandomEntity(random, _rowGroups);
                },
                GetColumnValues = (houseSaleData) =>
                {
                    return new[]
                    {
                        houseSaleData.NumberOfBeds.ToString(),
                        houseSaleData.SizeInSquareFeet.ToString(),
                        houseSaleData.SalePrice.ToString()
                    };
                }
            };

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
                        Name = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 1,
                        MaxValue = 1,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 1000,
                        MaxValue = 5000
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SalePrice,
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
                        Name = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 2,
                        MaxValue = 2,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 5000,
                        MaxValue = 9000
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SalePrice,
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
                        Name = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 3,
                        MaxValue = 3,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 10000,
                        MaxValue = 20000
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SalePrice,
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
                        Name = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 4,
                        MaxValue = 4,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 20000,
                        MaxValue = 30000,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SalePrice,
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
                        Name = CSVHouseSaleDataColumnNames.NumberOfBeds,
                        MinValue = 5,
                        MaxValue = 5,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SizeInSquareFeet,
                        MinValue = 30000,
                        MaxValue = 40000,
                    },
                    new CSVColumnConfig()
                    {
                        Name = CSVHouseSaleDataColumnNames.SalePrice,
                        MinValue = 600000,
                        MaxValue = 800000
                    },
                }
            });

            return rowGroups;
        }  
    }
}
