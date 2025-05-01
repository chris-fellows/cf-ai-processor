using CFAIProcessor.Common;
using CFAIProcessor.Logging;
using CFAIProcessor.Models;
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
        public void Run()
        {
            var logFile = "D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\Log\\Log.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(logFile));
            var log = new SimpleLogCSV(logFile);

            var config = new PredictionConfig
            {
                Name = "Linear Regression (Graph)",
                TrainingEpochs = 1000,
                LearningRate = 0.01f,
                Enabled = true,
                IsImportingGraph = false,
                TrainDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-train-v2.txt",
                TestDataFile = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\sales-test-v2.txt",
            };

            var predictionNew = new PredictionProcessorV2<HouseSaleData>();
            predictionNew.Run(config, log,
                            (dataFile) => GetFeatures(dataFile),
                            (dataFile) => GetLabels(dataFile));
            
            var prediction2 = new PredictionProcessorSaved<HouseSaleData>();
            prediction2.Run(config, log);

            var prediction = new PredictionProcessorV1<HouseSaleData>();
            prediction.Run(config, log);

            int xx = 1000;
        }

        private NDArray GetFeatures(string dataFile)
        {
            var items = GetData(dataFile);

            var features = np.zeros((items.Count(), 2), Tensorflow.TF_DataType.TF_FLOAT);

            var minNumberOfBeds = 1;
            var maxNumberOfBeds = 10;
            var minSquareFeed = 1;
            var maxSquareFeed = 1000000;
           
            int rowIndex = -1;
            foreach(var item in items)
            {
                rowIndex++;
                //features[rowIndex] = new[] { item.NumberOfBeds, item.SizeInSquareFeet };
                features[rowIndex] = new[]
                {
                    Convert.ToSingle(Normalize(item.NumberOfBeds, minNumberOfBeds, maxNumberOfBeds, 0, 1)),
                    Convert.ToSingle(Normalize(item.SizeInSquareFeet, minSquareFeed, maxSquareFeed, 0, 1))
                };
            }

            return features;
        }

        private NDArray GetLabels(string dataFile)
        {
            var items = GetData(dataFile);

            var labels = np.zeros((items.Count(), 1), Tensorflow.TF_DataType.TF_FLOAT);

            var minSalePrice = 1;
            var maxSalePrice = 10000000;

            int rowIndex = -1;
            foreach (var item in items)
            {
                rowIndex++;
                //labels[rowIndex] = new[] { item.SalePrice };
                labels[rowIndex] = new[]
                {
                    Convert.ToSingle(Normalize(item.SalePrice, minSalePrice, maxSalePrice, 0, 1))
                };
                //var shape = labels[rowIndex].shape;
                //int zzz = 1000;
            }

            return labels;
        }

        private List<HouseSaleData> GetData(string dataFile)
        {
            if (!File.Exists(dataFile))
            {
                throw new FileNotFoundException("Data file does not exist", dataFile);
            }

            var items = new List<HouseSaleData>();

            using (var reader = new StreamReader(dataFile))
            {
                int lineCount = 0;
                var headers = new List<string>();
                while (!reader.EndOfStream)
                {
                    lineCount++;
                    var line = reader.ReadLine();
                    var elements = line.Split('\t');
                    if (lineCount == 1)
                    {
                        headers = elements.ToList();
                    }
                    else
                    {
                        var item = new HouseSaleData()
                        {
                            NumberOfBeds = Convert.ToSingle(elements[headers.IndexOf("number_of_beds")]),
                            SizeInSquareFeet = Convert.ToSingle(elements[headers.IndexOf("size_in_square_feet")]),
                            SalePrice = Convert.ToSingle(elements[headers.IndexOf("sale_price")]),
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        private double Normalize(double val, double valmin, double valmax, double outputMin, double outputMax)
        {
            return (((val - valmin) / (valmax - valmin)) * (outputMax - outputMin)) + outputMin;
        }
    }
}
