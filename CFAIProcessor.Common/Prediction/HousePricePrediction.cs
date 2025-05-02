using CFAIProcessor.Constants;
using CFAIProcessor.CSV;
using CFAIProcessor.Logging;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow.NumPy;

namespace CFAIProcessor.Prediction
{
    /// <summary>
    /// Predicts house price because of learning data file of house sales
    /// </summary>
    public class HousePricePrediction
    {
        public void RunTrainModel(string trainDataFile, string trainConfigFile,
                        string testDataFile, string testConfigFile)
        {
            var logFile = "D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\Log\\Log.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(logFile));
            var log = new SimpleLogCSV(logFile);

            var config = new PredictionConfig
            {
                Name = "Linear Regression (Graph)",
                TrainingEpochs = 1000,
                LearningRate = 0.01f,
                IsImportingGraph = false,
                NormaliseValues = true,
                TrainDataFile = trainDataFile,
                MaxTrainRows = null,        // All training rows
                TestDataFile = testDataFile,
                MaxTestRows = null,         // All test rows
                ModelFolder = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel"
            };

            //var savePath = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel\\MyModel";

            // Set training data source
            var trainDataSource = new CSVPredictionDataSource(trainDataFile, trainConfigFile);

            // Set ttest data source
            var testDataSource = new CSVPredictionDataSource(testDataFile, testConfigFile);

            var predictionV3 = new PredictionProcessorV3();
            predictionV3.Run(config, trainDataSource, testDataSource);

            // Run model create
            var predictionNew = new PredictionProcessorV2();
            predictionNew.Run(config, log, trainDataSource, testDataSource);
                                        
            var prediction2 = new PredictionProcessorSaved<HouseSaleData>();
            prediction2.Run(config, log);

            var prediction = new PredictionProcessorV1();
            prediction.Run(config, log);

            int xx = 1000;
        }

        public void RunPredictModel(string trainDataFile, string trainConfigFile,
                   string testDataFile, string testConfigFile)
        {
            var logFile = "D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\Log\\Log.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(logFile));
            var log = new SimpleLogCSV(logFile);

            var config = new PredictionConfig
            {
                Name = "Linear Regression (Graph)",
                TrainingEpochs = 1000,
                LearningRate = 0.01f,
                IsImportingGraph = false,
                NormaliseValues = true,
                TrainDataFile = trainDataFile,
                TestDataFile = testDataFile,
                ModelFolder = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel"
            };
            
            var prediction2 = new PredictionProcessorSaved<HouseSaleData>();
            prediction2.Run(config, log);

            int xx = 1000;
        }

        //public NDArray GetFeatures(string dataFile, bool normalise)
        //{
        //    var items = GetData(dataFile);

        //    var features = np.zeros((items.Count(), 2), Tensorflow.TF_DataType.TF_FLOAT);

        //    var minNumberOfBeds = 1;
        //    var maxNumberOfBeds = 10;
        //    var minSquareFeet = 1;
        //    var maxSquareFeet = 1000000;

        //    int rowIndex = -1;
        //    foreach(var item in items)
        //    {
        //        rowIndex++;              
        //        features[rowIndex] = new[]
        //        {
        //            normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.NumberOfBeds, minNumberOfBeds, maxNumberOfBeds, 0, 1)) :
        //                    Convert.ToSingle(item.NumberOfBeds),
        //            normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.SizeInSquareFeet, minSquareFeet, maxSquareFeet, 0, 1)) : 
        //                    Convert.ToSingle(item.SizeInSquareFeet)
        //        };
        //    }

        //    return features;
        //}

        //public NDArray GetLabels(string dataFile, bool normalise)
        //{
        //    var items = GetData(dataFile);

        //    var labels = np.zeros((items.Count(), 1), Tensorflow.TF_DataType.TF_FLOAT);

        //    var minSalePrice = 1;
        //    var maxSalePrice = 10000000;

        //    int rowIndex = -1;
        //    foreach (var item in items)
        //    {
        //        rowIndex++;                
        //        labels[rowIndex] = new[]
        //        {
        //            normalise ? Convert.ToSingle(NumericUtilities.Normalize(item.SalePrice, minSalePrice, maxSalePrice, 0, 1)) : 
        //                Convert.ToSingle(item.SalePrice)
        //        };                
        //    }

        //    return labels;
        //}

        //private List<HouseSaleData> GetData(string dataFile)
        //{
        //    if (!File.Exists(dataFile))
        //    {
        //        throw new FileNotFoundException("Data file does not exist", dataFile);
        //    }

        //    var items = new List<HouseSaleData>();

        //    using (var reader = new StreamReader(dataFile))
        //    {
        //        int lineCount = 0;
        //        var headers = new List<string>();
        //        while (!reader.EndOfStream)
        //        {
        //            lineCount++;
        //            var line = reader.ReadLine();
        //            var elements = line.Split('\t');
        //            if (lineCount == 1)
        //            {
        //                headers = elements.ToList();
        //            }
        //            else
        //            {
        //                var item = new HouseSaleData()
        //                {
        //                    NumberOfBeds = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.NumberOfBeds)]),
        //                    SizeInSquareFeet = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SizeInSquareFeet)]),
        //                    SalePrice = Convert.ToSingle(elements[headers.IndexOf(CSVHouseSaleDataColumnNames.SalePrice)]),
        //                };
        //                items.Add(item);
        //            }
        //        }
        //    }

        //    return items;
        //}
    }
}
