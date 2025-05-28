using CFAIProcessor.Constants;
using CFAIProcessor.CSV;
using CFAIProcessor.ImageChecker;
using CFAIProcessor.ImageClassify;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.SystemTask
{
    /// <summary>
    /// Runs image classify training
    /// </summary>
    public class ImageTrainTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.RunImageTrain;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var imageClassifyModelService = (IImageClassifyModelService)serviceProvider.GetService(typeof(IImageClassifyModelService));
            var imageSetInfoService = (IImageSetInfoService)serviceProvider.GetService(typeof(IImageSetInfoService));
            var toastService = (IToastService)serviceProvider.GetService(typeof(IToastService));

            var imageClassifier = new ImageClassifierV2();

            var imageTrainConfig = (ImageTrainConfig)parameters["ImageTrainConfig"];

            toastService.Information($"Training model");

            var imageClassifyModel = imageClassifier.Train(imageTrainConfig, cancellationToken);

            await imageClassifyModelService.AddAsync(imageClassifyModel);

            toastService.Information($"Trained model");
        }
    }
}
