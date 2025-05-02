using CFAIProcessor.Constants;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.SystemTask
{
    public class RunPredictionTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.RunPrediction;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {            
            var predictionRequest = (PredictionRequest)parameters["PredictionRequest"];

            var predictionRequestProcessor = serviceProvider.GetService<IPredictionRequestProcessor>();

            predictionRequestProcessor.Run(predictionRequestProcessor, cancellationToken);
        }
    }
}
