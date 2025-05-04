using CFAIProcessor.CSV;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System.IO.Compression;
using Tensorflow;

namespace CFAIProcessor.Prediction
{
    /// <summary>
    /// Runs prediction request locally
    /// </summary>
    public class LocalPredictionRequestProcessor : IPredictionRequestProcessor
    {
        private class ConfigParams
        {
            public string TrainDataFile { get; set; } = String.Empty;

            public CSVConfig TrainConfig { get; set; } = new();

            public string TestDataFile { get; set; } = String.Empty;

            public CSVConfig TestConfig { get; set; } = new();

            public PredictionConfig PredictionConfig { get; set; } = new();

            public string PredictionOutputDataFile { get; set; } = String.Empty;
        }

        public PredictionResponse Run(PredictionRequest predictionRequest, CancellationToken cancellationToken)
        {
            var predictionResponse = new PredictionResponse() { Id = Guid.NewGuid().ToString() };

            // Prepare input files
            var (configParams, errorMessage) = PrepareInputFiles(predictionRequest.InputFile);

            // Run prediction
            if (String.IsNullOrEmpty(errorMessage))
            {
                RunPrediction(configParams, predictionRequest, predictionResponse, cancellationToken);
            }
            else
            {
                predictionResponse.ErrorMessage = errorMessage;
            }

            return predictionResponse;
        }

        private void RunPrediction(ConfigParams configParams,
                                PredictionRequest predictionRequest, 
                                PredictionResponse predictionResponse, 
                                CancellationToken cancellationToken)
        {
            //var config = new PredictionConfig
            //{
            //    Name = "Linear Regression (Graph)",
            //    TrainingEpochs = 1000,
            //    LearningRate = 0.01f,
            //    IsImportingGraph = false,
            //    NormaliseValues = true,
            //    //TrainDataFile = trainDataFile,
            //    MaxTrainRows = null,        // All training rows
            //    //TestDataFile = testDataFile,
            //    MaxTestRows = null,         // All test rows
            //    ModelFolder = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel"
            //};

            //var savePath = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel\\MyModel";

            // Create data source for reading training data
            var trainDataSource = new CSVPredictionDataSource(configParams.TrainDataFile, configParams.TrainConfig);

            // Create data source for reading training data
            var testDataSource = new CSVPredictionDataSource(configParams.TestDataFile, configParams.TestConfig);

            // Create prediction output           
            //var predictionOutputFile = new CSVPredictionOutputFile(configParams.PredictionOutputDataFile, configParams.TrainConfig);
            var predictionOutputFile = new CSVDataSetWriter(configParams.PredictionOutputDataFile, (Char)9);

            var predictionV3 = new PredictionProcessorV3();
            predictionV3.TrainAndPredict(configParams.PredictionConfig, trainDataSource, testDataSource, predictionOutputFile, cancellationToken);
        }

        /// <summary>
        /// Prepares input files from .zip file:
        /// - Training data file.
        /// - Training data config file (Lists CSV columns).
        /// - Test data file.
        /// - Test data config file (Lists CSV columns).
        /// - Prediction config
        /// </summary>
        /// <param name="inputFile"></param>
        private (ConfigParams configParamsOutput,
                string errorMessageOutput) PrepareInputFiles(string inputFile)
        {
            var configParams = new ConfigParams()
            {
                PredictionOutputDataFile = Path.Combine(Path.GetTempPath(), "prediction.txt")
            };
            var errorMessage = "";

            if (inputFile.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))    // .zip file, extract files
            {
                // Create temp folder to extract files to
                string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempFolder);

                // Clean up temp folder on completion
                //session.AddAction(() => IOUtilities.DeleteDirectory(tempFolder));

                // Unzip files
                using (var archive = ZipFile.Open(inputFile, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(tempFolder);
                    archive.Dispose();
                }

                // Get files                
                var files = Directory.GetFiles(tempFolder, "*.*").ToArray();
                foreach(var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName.StartsWith("train-data-", StringComparison.InvariantCultureIgnoreCase) &&
                        fileName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))                   
                    {
                        // Training CSV config
                        configParams.TrainConfig = JsonUtilities.DeserializeFromString<CSVConfig>(File.ReadAllText(fileName, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
                    }
                    else if (fileName.StartsWith("train-data-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Training CSV
                        configParams.TrainDataFile = fileName;
                    }
                    else if (fileName.StartsWith("test-data-", StringComparison.InvariantCultureIgnoreCase) &&
                            fileName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Test CSV config                        
                        configParams.TestConfig = JsonUtilities .DeserializeFromString<CSVConfig>(File.ReadAllText(fileName, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
                    }
                    else if (fileName.StartsWith("test-data-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Test CSV
                        configParams.TestDataFile = fileName;
                    }
                    else if (fileName.Equals("prediction-config.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Prediction config
                        configParams.PredictionConfig = JsonUtilities.DeserializeFromString<PredictionConfig>(File.ReadAllText(fileName, System.Text.Encoding.UTF8), JsonUtilities.DefaultJsonSerializerOptions);
                    }
                }
            }

            if (String.IsNullOrEmpty(configParams.TrainDataFile))
            {
                errorMessage = "Training data file is missing";
            }
            else if (configParams.TrainConfig.Columns.Count == 0)
            {
                errorMessage = "Training data config file is invalid or missing";
            }
            else if (String.IsNullOrEmpty(configParams.TestDataFile))
            {
                errorMessage = "Test data file is missing";
            }
            else if (configParams.TestConfig.Columns.Count == 0)
            {
                errorMessage = "Test data config file is invalid or missing";
            }
            else if (configParams.PredictionConfig.TrainingEpochs < 1)
            {
                errorMessage = "Prediction config file is invalid or missing";
            }

            return (configParams, errorMessage);
        }
    }
}
