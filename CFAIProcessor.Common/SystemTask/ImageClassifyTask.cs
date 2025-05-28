using CFAIProcessor.Constants;
using CFAIProcessor.ImageClassify;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.SystemTask
{
    public class ImageClassifyTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.RunImageClassify;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var predictionModelService = (IPredictionModelService)serviceProvider.GetService(typeof(IPredictionModelService));
            var dataSetInfoService = (IDataSetInfoService)serviceProvider.GetService(typeof(IDataSetInfoService));
            var toastService = (IToastService)serviceProvider.GetService(typeof(IToastService));

            var imageClassifier = new ImageClassifierV2();

            var imageClassifyConfig = (ImageClassifyConfig)parameters["ImageClassifyConfig"];

            toastService.Information($"Classifying images");

            imageClassifier.Classify(imageClassifyConfig, cancellationToken);

            toastService.Information($"Classifying images");
        }
    }
}
