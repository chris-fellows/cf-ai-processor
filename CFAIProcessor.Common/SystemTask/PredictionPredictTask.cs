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

namespace CFAIProcessor.SystemTask
{
    public class PredictionPredictTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.RunPredictionPredict;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var predictionModelService = (IPredictionModelService)serviceProvider.GetService(typeof(IPredictionModelService));
            var dataSetInfoService = (IDataSetInfoService)serviceProvider.GetService(typeof(IDataSetInfoService));
            var toastService = (IToastService)serviceProvider.GetService(typeof(IToastService));

            var predictionProcessor = new PredictionProcessorV3();

            var predictionPredictConfig = (PredictionPredictConfig)parameters["PredictionPredictConfig"];

            toastService.Information("Predicting model");

            var dataSetInfo = dataSetInfoService.GetById(predictionPredictConfig.DataSetInfoId);

            // TODO: Clean this up
            var csvConfig = new CSVConfig()
            {
                Columns = predictionPredictConfig.ColumnConfigs.Select(column =>
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

            var testDataSource = new CSVPredictionDataSource(dataSetInfo.DataSource, csvConfig);

            var predictionDataSetWriter = new CSVDataSetWriter(predictionPredictConfig.DataSetInfoDataSource, (Char)9, Encoding.UTF8);            

            //var configFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Data Sets\\test-data-house-sales.json";
            //var validCSVConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(configFile, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
            //var validDataSource = new CSVPredictionDataSource("D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Data Sets\\test-data-house-sales.txt", validCSVConfig);

            var predictDataSetInfo = predictionProcessor.Predict(predictionPredictConfig, testDataSource, predictionDataSetWriter, cancellationToken);            

            toastService.Information("Predict complete");
        }
    }
}
