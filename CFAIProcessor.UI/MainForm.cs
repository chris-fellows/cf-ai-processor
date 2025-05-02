using CFAIProcessor.CSV;
using CFAIProcessor.ImageChecker;
using CFAIProcessor.Prediction;

namespace CFAIProcessor.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //CreateHouseSalePredictData();

            int xxx = 1000;
        }

        private void CreateHouseSalePredictData()
        {
            // Create training data
            new CSVHouseSaleDataCreator().Create(@"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2.txt",
                                            @"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2-config.json",
                                            '\t', 1000);

            // Create test data
            new CSVHouseSaleDataCreator().Create(@"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2.txt",
                                                @"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2-config.json",
                                            '\t', 100);
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            var trainDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2.txt";
            var trainConfigFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2-config.json";

            var testDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2.txt";
            var testConfigFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2-config.json";

            var prediction = new HousePricePrediction();

            prediction.Run(trainDataFile, trainConfigFile,
                        testDataFile, testConfigFile);

            int xxx = 1000;
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            var checker = new ImageClassChecker();

            checker.Run();

            int xxx = 1000;
        }
    }
}
