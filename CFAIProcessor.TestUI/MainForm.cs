using CFAIProcessor.CSV;
using CFAIProcessor.Enums;
using CFAIProcessor.ImageClassify;
using CFAIProcessor.Models;
using CFAIProcessor.Prediction;
using CFAIProcessor.Services;
using CFAIProcessor.Utilities;
using CFCSV.Reader;
using CFCSV.Writer;
using System.Text;
using Tensorflow;

namespace CFAIProcessor.TestUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //TestAggregateCSV();

            //TestWeightedRandom();

            //string folder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local";
            //CreateHouseSalePredictData(folder);       
        }

        private void TestAggregateCSV()
        {
            // Note that GroupByColumnInternalNames isn't currently used because we group by columns with
            // AggregateColumn.AggregateAction = None.
            var aggregateConfig = new AggregateConfig()
            {
                Columns = new List<AggregateColumn>()
                {
                   //new AggregateColumn()
                   //{
                   //    InputName = "number_of_beds",
                   //    OutputName = "number_of_beds",
                   //    AggregateAction = AggregateActions.None
                   //},
                   new AggregateColumn()
                   {
                       InputName = "size_in_square_feet",
                       OutputName = "size_in_square_feet_rounded",
                       AggregateAction = AggregateActions.None,
                       NumberConvertAction = NumberConvertActions.ModuloRoundDown,
                       NumberConvertModuloValue = 1000     // Round to multiples of
                   },
                    new AggregateColumn()
                   {
                       InputName = "size_in_square_feet",
                       OutputName = "size_in_square_feet_avg",
                       AggregateAction = AggregateActions.Avg,
                       DecimalPlaces = 0,
                       GroupByColumnInternalNames = new()
                       {
                           "size_in_square_feet_rounded"
                       }
                   },
                   new AggregateColumn()
                   {
                       InputName = "size_in_square_feet",
                       OutputName = "size_in_square_feet_min",
                       AggregateAction = AggregateActions.Min,
                       DecimalPlaces = 0,
                       GroupByColumnInternalNames = new()
                       {
                           "size_in_square_feet_rounded"
                       }
                   },
                   new AggregateColumn()
                   {
                       InputName = "size_in_square_feet",
                       OutputName = "size_in_square_feet_max",
                       AggregateAction = AggregateActions.Max,
                       DecimalPlaces = 0,
                       GroupByColumnInternalNames = new()
                       {
                           "size_in_square_feet_rounded"
                       }
                   },
                   new AggregateColumn()
                   {
                       InputName = "sale_price",
                       OutputName = "sale_price_actual_avg",
                       AggregateAction = AggregateActions.Avg,
                       DecimalPlaces = 0,
                       GroupByColumnInternalNames = new()
                       {
                           "size_in_square_feet_rounded"
                       }
                   },
                   new AggregateColumn()
                   {
                       InputName = "prediction_0",
                       OutputName = "sale_price_predicted_avg",
                       AggregateAction = AggregateActions.Avg,
                       DecimalPlaces = 0,
                       GroupByColumnInternalNames = new()
                       {
                           "size_in_square_feet_rounded"
                       }
                   },
                }
            };

            var file = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\predict-output.txt";

            var aggregateService = new AggregateService();

            var dataTable = new CSVDataTableReader().Read(file);

            var dataTableNew = aggregateService.Aggregate(dataTable, aggregateConfig);

            var aggregateFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\predict-aggregate.txt";
            var aggregateConfigFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\predict-aggregate.json";
            SaveDataTable(dataTableNew, aggregateFile, (Char)9, aggregateConfigFile);

            int xxx = 100;
        }

        private static void SaveDataTable(System.Data.DataTable dataTable, string file, Char delimiter, string csvConfigFile)
        {
            using (var writer = new StreamWriter(file, true, System.Text.Encoding.UTF8))
            {
                var csvConfig = new CSVConfig()
                {

                };

                var line = new StringBuilder("");
                for (int index = 0; index < dataTable.Columns.Count; index++)
                {
                    if (line.Length > 0) line.Append(delimiter);
                    line.Append(dataTable.Columns[index].ColumnName);

                    csvConfig.Columns.Add(new CSVColumnConfig()
                    {
                        InternalName = dataTable.Columns[index].ColumnName,
                        ExternalName = dataTable.Columns[index].ColumnName
                    });
                }
                writer.WriteLine(line.ToString());

                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    line.Length = 0;
                    for (int index = 0; index < dataTable.Columns.Count; index++)
                    {
                        if (line.Length > 0) line.Append(delimiter);
                        line.Append(dataTable.Rows[rowIndex][index]);
                    }
                    writer.WriteLine(line.ToString());
                }

                File.WriteAllText(csvConfigFile, JsonUtilities.SerializeToString(csvConfig, JsonUtilities.DefaultJsonSerializerOptions));
            }
        }

        private void TestWeightedRandom()
        {
            var items = new List<string>() { "One", "Two", "Three", "Four", "Five" };
            var proportions = new List<double>() { 0.1, 0.1, 0.60, 0.1, 0.1 };
            var countsByItem = new Dictionary<string, int>();
            foreach (var item in items)
            {
                countsByItem.Add(item, 0);
            }

            for (int index = 0; index < 10000; index++)
            {
                var random = NumericUtilities.GetWeightedRandom(items, proportions);
                countsByItem[random] += 1;
            }

            var xxx = 1000;
        }

        private void CreateHouseSalePredictData(string folder)
        {
            // Create training data
            new CSVHouseSaleDataCreator().Create(Path.Combine(folder, "train-data-house-sales.txt"),
                                             Path.Combine(folder, "train-data-house-sales.json"),
                                            '\t', 1000, null);

            // Create test data
            new CSVHouseSaleDataCreator().Create(Path.Combine(folder, "test-data-house-sales.txt"),
                                            Path.Combine(folder, "test-data-house-sales.json"),
                                            '\t', 100, null);

            //// Create prediction data
            //new CSVHouseSaleDataCreator().Create(Path.Combine(folder, "prediction-data-house-sales.txt"),
            //                                Path.Combine(folder, "prediction-data-house-sales.json"),
            //                                '\t', 100, null);

            // Save prediction config
            var predictionConfigFile = Path.Combine(folder, "prediction-config.json");
            var predictionConfig = new PredictionConfig()
            {
                Name = "House Sales Linear Regression (Graph)",
                TrainingEpochs = 1000,
                LearningRate = 0.01f,
                IsImportingGraph = false,
                NormaliseValues = true,
                MaxTrainRows = null,        // All training rows                
                MaxTestRows = null,         // All test rows
                ModelFolder = ""
            };
            File.WriteAllText(predictionConfigFile, JsonUtilities.SerializeToString(predictionConfig, JsonUtilities.DefaultJsonSerializerOptions), System.Text.Encoding.UTF8);
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            string folder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local";

            var trainDataFile = Path.Combine(folder, "train-data-house-sales.txt");
            var trainConfigFile = Path.Combine(folder, "train-data-house-sales.json");

            var testDataFile = Path.Combine(folder, "test-data-house-sales.txt");
            var testConfigFile = Path.Combine(folder, "test-data-house-sales.json");

            var predictFile = Path.Combine(folder, "predict-output.txt");

            var prediction = new HousePricePrediction();

            var cancellationTokenSource = new CancellationTokenSource();

            prediction.RunTrainAndPredictModel(trainDataFile, trainConfigFile,
                        testDataFile, testConfigFile,
                        predictFile, cancellationTokenSource.Token);

            int xxx = 1000;
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            var trainDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2.txt";
            var trainConfigFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2-config.json";

            var testDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2.txt";
            var testConfigFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2-config.json";

            var prediction = new HousePricePrediction();

            prediction.RunPredictModel(trainDataFile, trainConfigFile,
                        testDataFile, testConfigFile);

            int xxx = 1000;
        }

        private void btnImageClassify_Click(object sender, EventArgs e)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var catsAndDogsImageClassify = new CatAndDogImageClassify();

            catsAndDogsImageClassify.RunTrainAndPredictModel(cancellationTokenSource.Token);

            int xxx = 1000;

        }
    }
}
