using CFAIProcessor.CSV;
using CFAIProcessor.Prediction;

namespace CFAIProcessor.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //new CSVHouseSaleDataCreator().Create(@"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2.txt", '\t', 1000);
            //new CSVHouseSaleDataCreator().Create(@"D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2.txt", '\t', 100);            
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            var prediction = new HousePricePrediction();

            prediction.Run();

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
