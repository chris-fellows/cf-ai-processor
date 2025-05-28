using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.ImageClassify
{
    public class CatAndDogImageClassify
    {
        public void RunTrainAndPredictModel(CancellationToken cancellationToken)
        {
            var config = new ImageClassifyConfigV1()
            {
                AllImageFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Cats & Dogs dataset\\kagglecatsanddogs_5340\\PetImages",
                TrainImageFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Cats & Dogs Test\\train",
                TrainImageCount = 500,
                ValidImageFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Cats & Dogs Test\\valid",
                ValidImageCount = 100,
                TestImageFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Cats & Dogs Test\\test",
                TestImageCount = 50,
                ClassNames = new List<string>() { "cat", "dog" },
                TrainEpochs = 10
            };

            var imageClassifier = new ImageClassifierV1();

            imageClassifier.TrainAndClassify(config);
        }
    }
}
