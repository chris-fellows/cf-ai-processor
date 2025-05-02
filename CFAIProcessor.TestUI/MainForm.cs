using CFAIProcessor.CSV;
using CFAIProcessor.ImageChecker;
using CFAIProcessor.Models;
using CFAIProcessor.Prediction;
using CFAIProcessor.Utilities;
using Tensorflow;

namespace CFAIProcessor.TestUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            string folder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local";
            //CreateHouseSalePredictData(folder);

            int xxx = 1000;
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

            var prediction = new HousePricePrediction();

            prediction.RunTrainModel(trainDataFile, trainConfigFile,
                        testDataFile, testConfigFile);

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
    }
}
