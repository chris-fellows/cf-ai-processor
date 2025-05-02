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
using CFAIProcessor.Interfaces;

namespace CFAIProcessor.Prediction
{
    /// <summary>
    /// Runs prediction from data source
    /// </summary>
    internal class PredictionProcessorV2
    {        
        private int _displayStep = 50;
      
        public bool Run(PredictionConfig predictionConfig, ISimpleLog log,
                        IPredictionDataSource trainDataSource,
                        IPredictionDataSource testDataSource)
        {
            tf.compat.v1.disable_eager_execution();

            NDArray _trainX = trainDataSource.GetFeatureValues(predictionConfig.NormaliseValues);
            NDArray _trainY = trainDataSource.GetLabelValues(predictionConfig.NormaliseValues);            
            var trainItemCount = (int)_trainX.shape[0];
            var trainFeatureCount = (int)_trainX.shape[1];
            var trainLabelCount = (int)_trainY.shape[1];            

            // tf Graph Input
            var X = tf.placeholder(tf.float32, shape: (1, trainFeatureCount), name: "X");
            var Y = tf.placeholder(tf.float32, shape: (trainLabelCount), name: "Y");

            // Set model weights 
            // We can set a fixed init value in order to debug
            // var rnd1 = rng.randn<float>();
            // var rnd2 = rng.randn<float>();
            //var W = tf.Variable(-0.06f, name: "weight");
            //var b = tf.Variable(-0.73f, name: "bias");
            var initialWeightValues = new float[trainFeatureCount];
            for(int index =0; index < initialWeightValues.Length; index++)
            {
                initialWeightValues[index] = -0.06f;
            }            
            var W = tf.Variable(initialWeightValues, name: "weight");
            //var W = tf.Variable(-0.06f, name: "weight");
            var b = tf.Variable(new[] { -0.73f }, name: "bias");

            // Construct a linear model
            var prediction = tf.add(tf.multiply(X, W), b, name: "prediction");        
            log.Log(DateTimeOffset.UtcNow, "Information", $"pred.name={prediction.name}");

            // Set cost function (Mean squared error)
            var cost = tf.reduce_sum(tf.pow(prediction - Y, 2.0f), name: "cost") / (2.0f * trainItemCount);            
            log.Log(DateTimeOffset.UtcNow, "Information", $"cost.name={cost.name}");            

            // Gradient descent
            // Note, minimize() knows to modify W and b because Variable objects are trainable=True by default
            var optimizer = tf.train.GradientDescentOptimizer(predictionConfig.LearningRate).minimize(cost);
            log.Log(DateTimeOffset.UtcNow, "Information", $"optimizer.name={optimizer.name}");

            // Initialize the variables (i.e. assign their default value)
            var init = tf.global_variables_initializer();
            log.Log(DateTimeOffset.UtcNow, "Information", $"init.name={init.name}");

            // Start training
            using var session = tf.Session();
            // Run the initializer
            session.run(init);            

            // Fit all training data
            for (int epoch = 0; epoch < predictionConfig.TrainingEpochs; epoch++)
            {               
                for(int itemIndex = 0; itemIndex < _trainX.shape[0]; itemIndex++)
                {
                    var trainX_item = _trainX[itemIndex];
                    var trainY_item = _trainY[itemIndex];

                    //var original0 = trainDataSource.GetNonNormalisedFeatureValue(0, trainX_item[0]);
                    //var original1 = trainDataSource.GetNonNormalisedFeatureValue(1, trainX_item[1]);

                    session.run(optimizer, (X, trainX_item), (Y, trainY_item));

                    //log.Log(DateTimeOffset.UtcNow, "Information", $"Epoch: {epoch + 1} itemIIndex={itemIndex} " + $"W={session.run(W)} b={session.run(b)}");

                    /* Works
                    var feedDict = new FeedDict();
                    feedDict.Add(X, train_X_item);
                    feedDict.Add(Y, train_Y_item);
                    session.run(optimizer, feedDict);
                    int xxx = 1000;
                    */

                    /* Doesn't work
                    var parameters = new Dictionary<string, object>()
                    {
                        { "x", train_X_item },
                        { "y", train_Y_item }
                    };                   
                    session.run(optimizer, new FeedItem("x", train_X_item), new FeedItem("x", train_Y_item));                    
                    */
                }

                //foreach (var (x, y) in zip<float>(train_X, train_Y))
                //{
                //    int xzzzz = 1000;
                //    session.run(optimizer, (X, x), (Y, y));
                //}

                // Display logs per epoch step
                if ((epoch + 1) % _displayStep == 0)
                {
                    var c = session.run(cost, (X, _trainX), (Y, _trainY));                    
                    log.Log(DateTimeOffset.UtcNow, "Information", $"Epoch: {epoch + 1}, Cost={c}, Weight={session.run(W)} Bias={session.run(b)}");
                }
            }

            log.Log(DateTimeOffset.UtcNow, "Information", "Optimization Finished!");
            var trainCost = session.run(cost, (X, _trainX), (Y, _trainY));
            log.Log(DateTimeOffset.UtcNow, "Information", $"Training cost={trainCost}, Weight={session.run(W)}, Bias={session.run(b)}, Name={trainCost.name}");

            // Testing example      
            var testX = testDataSource.GetFeatureValues(predictionConfig.NormaliseValues);
            var testY = testDataSource.GetLabelValues(predictionConfig.NormaliseValues);
            
            //var predictionResult = session.run(prediction, (X, testX), (Y, testY));
            
            log.Log(DateTimeOffset.UtcNow, "Information", "Testing... (Mean square loss Comparison)");
            var testCost = session.run(tf.reduce_sum(tf.pow(prediction - Y, 2.0f)) / (2.0f * testX.shape[0]),
                (X, testX), (Y, testY));
            log.Log(DateTimeOffset.UtcNow, "Information", $"Testing cost={testCost}, name={testCost.name}");
            var difference = Math.Abs((float)trainCost - (float)testCost);
            log.Log(DateTimeOffset.UtcNow, "Information", $"Absolute mean square loss difference: {difference}");

            //var myTestCost = tf.reduce_sum(tf.pow(prediction - Y, 2.0f)) / (2.0f * testX.shape[0]);
            //for (int itemIndex = 0; itemIndex < testX.shape[0]; itemIndex++)
            //{
            //    var testX_item = testX[itemIndex];
            //    var testY_item = testY[itemIndex];
            //    var myTestResult = session.run(myTestCost, (X, testX_item), (Y, testY_item));
            //    int zzz = 1000;
            //}

            //var savePath = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel\\MyModel";
            //var saver = tf.train.Saver();
            //saver.save(session, savePath);

            return difference < 0.01;
        }                
    }
}
