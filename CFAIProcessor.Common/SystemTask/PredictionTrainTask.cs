using CFAIProcessor.Constants;
using CFAIProcessor.CSV;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Prediction;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CFAIProcessor.SystemTask
{
    /// <summary>
    /// Runs prediction training
    /// </summary>
    public class PredictionTrainTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.RunPredictionTrain;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var predictionModelService = (IPredictionModelService)serviceProvider.GetService(typeof(IPredictionModelService));
            var dataSetInfoService = (IDataSetInfoService)serviceProvider.GetService(typeof(IDataSetInfoService));
            var toastService = (IToastService)serviceProvider.GetService(typeof(IToastService));

            var predictionProcessor = new PredictionProcessorV3();            

            var predictionTrainConfig = (PredictionTrainConfig)parameters["PredictionTrainConfig"];

            toastService.Information($"Training model {predictionTrainConfig.ModelName}");

            var dataSetInfo = dataSetInfoService.GetById(predictionTrainConfig.DataSetInfoId);

            // TODO: Clean this up
            var csvConfig = new CSVConfig()
            {
                Columns = predictionTrainConfig.ColumnConfigs.Select(column =>
                {
                    return new CSVColumnConfig()
                    {
                       ExternalName = column.ExternalName,
                       InternalName = column.InternalName,
                       IsFeature = column.IsFeature,
                       IsLabel = column.IsLabel,
                        MaxValue = column.MaxValue,
                        MinValue = column.MinValue
                    };
                }).ToList()
            };

            // TODO: Clean this up
            //var configFile = Path.Combine(Path.GetDirectoryName(dataSetInfo.DataSource), $"{Path.GetFileNameWithoutExtension(dataSetInfo.DataSource)}.json");
            //var csvConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(configFile, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
           
            var predictionDataSource = new CSVPredictionDataSource(dataSetInfo.DataSource, csvConfig);


            var configFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Data Sets\\test-data-house-sales.json";
            var validCSVConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(configFile, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
            var validDataSource = new CSVPredictionDataSource("D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Data Sets\\test-data-house-sales.txt", validCSVConfig);                                                        

            var predictionModel = predictionProcessor.Train(predictionTrainConfig, predictionDataSource, validDataSource, cancellationToken);

            await predictionModelService.AddAsync(predictionModel);

            toastService.Information($"Trained model {predictionTrainConfig.ModelName}");
        }
    }
}
