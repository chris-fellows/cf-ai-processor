using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow.NumPy;
using static Tensorflow.Keras.Engine.InputSpec;
using static Tensorflow.Binding;
using CFAIProcessor.Logging;
using Tensorflow;
using Tensorflow.Sessions;
using PandasNet;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras;
using CFAIProcessor.Interfaces;

namespace CFAIProcessor.Prediction
{
    /// <summary>
    /// Prediction processor via Keras
    /// </summary>
    internal class PredictionProcessorV3
    {
        public void Run(PredictionConfig predictionConfig,
                        IPredictionDataSource trainDataSource,
                        IPredictionDataSource testDataSource,
                        IPredictionOutputFile predictionOutputFile,
                        CancellationToken cancellationToken)
        {
            // https://scisharp.github.io/Keras.NET/
            var model = new Sequential(new Tensorflow.Keras.ArgsDefinition.SequentialArgs()
            {
                Layers = new List<Tensorflow.Keras.ILayer>()
                {
                    new Tensorflow.Keras.Layers.Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs()
                    {
                        Units = 64,
                        InputShape = (1,2),
                        Activation = new Tensorflow.Keras.Activations().Tanh
                    }),
                    new Tensorflow.Keras.Layers.Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs()
                    {
                        Units = 1,                        
                        Activation = new Tensorflow.Keras.Activations().Tanh
                    })
                }
            });

            // Get training data
            var trainX = trainDataSource.GetFeatureValues(predictionConfig.NormaliseValues, predictionConfig.MaxTrainRows);
            var trainY = trainDataSource.GetLabelValues(predictionConfig.NormaliseValues, predictionConfig.MaxTrainRows);
            
            // Compile
            model.compile(optimizer: new Tensorflow.Keras.Optimizers.Adam(predictionConfig.LearningRate), 
                            loss: new Tensorflow.Keras.Losses.MeanSquaredError());

            var weightsOriginal = model.get_weights();            

            // Train model            
            model.fit(trainX, trainY, batch_size: 100, epochs: predictionConfig.TrainingEpochs);

            var weightsNew = model.get_weights();

            // Get test data
            var testX = testDataSource.GetFeatureValues(predictionConfig.NormaliseValues, predictionConfig.MaxTestRows);
            var testY = testDataSource.GetLabelValues(predictionConfig.NormaliseValues, predictionConfig.MaxTestRows);

            // Test prediction
            //var testX = trainX[0];
            var prediction = model.predict(testX);

            // Check prediction results.
            // To predict CSV then we write test features, test labels & predicted labels
            var predictionValues = prediction.numpy();
            int rowIndex = -1;

            float totalPredictedValue = 0;
            float totalActualValue = 0;
            foreach(var predictedValueCurrent in predictionValues)
            {
                rowIndex++;

                // Set row features (Actual)
                var rowFeatures = new List<float>();
                for (int featureIndex = 0; featureIndex < testX[rowIndex].shape.dims[0]; featureIndex++)
                {
                    var featureValue = (float)testX[rowIndex][featureIndex];
                    if (predictionConfig.NormaliseValues)
                    {
                        featureValue = testDataSource.GetNonNormalisedFeatureValue(featureIndex, featureValue);
                    }
                    rowFeatures.Add(featureValue);
                }

                // Set row labels (Actual)                
                var rowLabels = new List<float>();
                for (int labelIndex = 0; labelIndex < testY[rowIndex].shape.dims[0]; labelIndex++)
                {
                    var labelValue = (float)testY[rowIndex][labelIndex];
                    if (predictionConfig.NormaliseValues)
                    {
                        labelValue = testDataSource.GetNonNormalisedLabelValue(labelIndex, labelValue);
                    }
                    rowLabels.Add(labelValue);
                }

                // Check predicted vs actual
                var rowLabelsPredicted = new List<float>();               
                for (var labelIndex = 0; labelIndex < predictedValueCurrent.shape.dims[0]; labelIndex++)
                {
                    float predictedValue = predictedValueCurrent[labelIndex];
                    float actualValue = testY[rowIndex][labelIndex];

                    // Convert normalised values to non-normalised
                    if (predictionConfig.NormaliseValues)
                    {
                        predictedValue = testDataSource.GetNonNormalisedLabelValue(0, predictedValue);
                        actualValue = trainDataSource.GetNonNormalisedLabelValue(0, actualValue);
                    }

                    totalPredictedValue += predictedValue;
                    totalActualValue += actualValue;

                    rowLabelsPredicted.Add(predictedValue);

                    var valueDifference = Math.Abs(predictedValue - actualValue);                    
                }

                // Write results to prediction output
                predictionOutputFile.Write(rowFeatures.ToArray(), rowLabels.ToArray(), rowLabelsPredicted.ToArray());
            }

            var averageDiff = Math.Abs(totalPredictedValue - totalActualValue) / rowIndex + 1;

            // Save model
            if (!String.IsNullOrEmpty(predictionConfig.ModelFolder))
            {
                model.save(predictionConfig.ModelFolder);
            }
            
            //https://github.com/TomasMantero/Predicting-House-Prices-Keras-ANN/blob/master/Predicting%20House%20Prices%20(Keras%20-%20ANN)%20(Version%205).ipynb                      
        }
    }
}
