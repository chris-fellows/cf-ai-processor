﻿using CFAIProcessor.Models;
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
        /// <summary>
        /// Trains model on training data and then predicts using the test data.
        /// </summary>
        /// <param name="predictionConfig"></param>
        /// <param name="trainDataSource"></param>
        /// <param name="testDataSource"></param>
        /// <param name="predictionDataSetWriter"></param>
        /// <param name="cancellationToken"></param>
        public void TrainAndPredict(PredictionConfig predictionConfig,
                        IPredictionDataSource trainDataSource,
                        IPredictionDataSource testDataSource,                        
                        IDataSetWriter predictionDataSetWriter,
                        //IPredictionOutputFile predictionOutputFile,
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
                        Activation = new Tensorflow.Keras.Activations().Tanh,
                        Trainable= true     // Added
                    }),
                    new Tensorflow.Keras.Layers.Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs()
                    {
                        Units = 1,                        
                        Activation = new Tensorflow.Keras.Activations().Tanh,
                        Trainable= true     // Added
                    })
                }
            });

            // Get training data
            var trainX = trainDataSource.GetFeatureValues(predictionConfig.NormaliseValues, predictionConfig.MaxTrainRows);
            var trainY = trainDataSource.GetLabelValues(predictionConfig.NormaliseValues, predictionConfig.MaxTrainRows);
            
            // Compile
            model.compile(optimizer: new Tensorflow.Keras.Optimizers.Adam(predictionConfig.LearningRate),                
                            loss: new Tensorflow.Keras.Losses.MeanSquaredError(),
                            metrics: new[] { "accuracy"});

            var weightsOriginal = model.get_weights();            
            
            // Train model            
            model.fit(trainX, trainY, batch_size: 100, epochs: predictionConfig.TrainingEpochs);

            var metrics = model.metrics.ToList();

            var weightsNew = model.get_weights();

            // Save model
            if (!String.IsNullOrEmpty(predictionConfig.ModelFolder))
            {
                model.save(predictionConfig.ModelFolder);
            }

            // Get test data
            var testX = testDataSource.GetFeatureValues(predictionConfig.NormaliseValues, predictionConfig.MaxTestRows);
            var testY = testDataSource.GetLabelValues(predictionConfig.NormaliseValues, predictionConfig.MaxTestRows);

            // Test prediction
            var prediction = model.predict(testX);

            // Check prediction results.
            // To predict CSV then we write test features, test labels & predicted labels
            var predictionValues = prediction.numpy();
            int rowIndex = -1;
            float totalPredictedValue = 0;
            float totalActualValue = 0;
            var testFeatureNames = testDataSource.FeatureNames;
            var testLabelNames = testDataSource.LabelNames;          
            foreach(var predictedValueCurrent in predictionValues)
            {
                rowIndex++;

                var predictionResultRow = new Dictionary<string, string>();

                // Set row features (Actual)
                //var rowFeatures = new List<float>();
                for (int featureIndex = 0; featureIndex < testX[rowIndex].shape.dims[0]; featureIndex++)
                {
                    var featureValue = (float)testX[rowIndex][featureIndex];
                    if (predictionConfig.NormaliseValues)
                    {
                        featureValue = testDataSource.GetNonNormalisedFeatureValue(featureIndex, featureValue);
                    }

                    //rowFeatures.Add(featureValue);

                    // Add feature to prediction row
                    predictionResultRow.Add(testFeatureNames[featureIndex], featureValue.ToString());                    
                }

                // Set row labels (Actual)                
                //var rowLabels = new List<float>();
                for (int labelIndex = 0; labelIndex < testY[rowIndex].shape.dims[0]; labelIndex++)
                {
                    var labelValue = (float)testY[rowIndex][labelIndex];
                    if (predictionConfig.NormaliseValues)
                    {
                        labelValue = testDataSource.GetNonNormalisedLabelValue(labelIndex, labelValue);
                    }

                    //rowLabels.Add(labelValue);

                    // Add label to prediction row
                    predictionResultRow.Add(testLabelNames[labelIndex], labelValue.ToString());
                }

                // Check predicted vs actual
                //var rowLabelsPredicted = new List<float>();               
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

                    //rowLabelsPredicted.Add(predictedValue);

                    // Add label prediction to prediction row
                    predictionResultRow.Add($"prediction_{labelIndex}", predictedValue.ToString());

                    // Add label prediction difference to prediction row
                    var valueDifference = Math.Abs(predictedValue - actualValue);
                    predictionResultRow.Add($"prediction_difference_{labelIndex}", valueDifference.ToString());
                }

                // Write results to prediction output
                predictionDataSetWriter.WriteRow(predictionResultRow);
                //predictionOutputFile.Write(rowFeatures.ToArray(), rowLabels.ToArray(), rowLabelsPredicted.ToArray());
            }

            var averageDiff = Math.Abs(totalPredictedValue - totalActualValue) / rowIndex + 1;
            
            //https://github.com/TomasMantero/Predicting-House-Prices-Keras-ANN/blob/master/Predicting%20House%20Prices%20(Keras%20-%20ANN)%20(Version%205).ipynb                      
        }
    }
}
