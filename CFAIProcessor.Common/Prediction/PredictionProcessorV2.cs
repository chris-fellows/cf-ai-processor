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
                        IPredictionDataSource dataSource)
        {
            tf.compat.v1.disable_eager_execution();

            NDArray _trainX = dataSource.GetFeatures(predictionConfig.TrainDataFile, predictionConfig.NormaliseValues);
            NDArray _trainY = dataSource.GetLabels(predictionConfig.TrainDataFile, predictionConfig.NormaliseValues);            
            int n_samples = (int)_trainX.shape[0];
            
            // tf Graph Input
            var X = tf.placeholder(tf.float32, shape: (1, 2), name: "X");
            var Y = tf.placeholder(tf.float32, shape: (1), name: "Y");

            // Set model weights 
            // We can set a fixed init value in order to debug
            // var rnd1 = rng.randn<float>();
            // var rnd2 = rng.randn<float>();
            //var W = tf.Variable(-0.06f, name: "weight");
            //var b = tf.Variable(-0.73f, name: "bias");
            var W = tf.Variable(new[] { -0.06f, -0.06f }, name: "weight");
            //var W = tf.Variable(-0.06f, name: "weight");
            var b = tf.Variable(new[] { -0.73f }, name: "bias");

            // Construct a linear model
            var pred = tf.add(tf.multiply(X, W), b, name: "pred");        
            log.Log(DateTimeOffset.UtcNow, "Information", $"pred.name={pred.name}");

            // Mean squared error
            var cost = tf.reduce_sum(tf.pow(pred - Y, 2.0f), name: "cost") / (2.0f * n_samples);            
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
                    var train_X_item = _trainX[itemIndex];
                    var train_Y_item = _trainY[itemIndex];
                    session.run(optimizer, (X, train_X_item), (Y, train_Y_item));

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
                    log.Log(DateTimeOffset.UtcNow, "Information", $"Epoch: {epoch + 1} cost={c}, W={session.run(W)} b={session.run(b)}");
                }
            }

            log.Log(DateTimeOffset.UtcNow, "Information", "Optimization Finished!");
            var training_cost = session.run(cost, (X, _trainX), (Y, _trainY));
            log.Log(DateTimeOffset.UtcNow, "Information", $"Training cost={training_cost} W={session.run(W)} b={session.run(b)}, name={training_cost.name}");

            // Testing example      
            var test_X = dataSource.GetFeatures(predictionConfig.TestDataFile, predictionConfig.NormaliseValues);
            var test_Y = dataSource.GetLabels(predictionConfig.TestDataFile, predictionConfig.NormaliseValues);

            log.Log(DateTimeOffset.UtcNow, "Information", "Testing... (Mean square loss Comparison)");
            var testing_cost = session.run(tf.reduce_sum(tf.pow(pred - Y, 2.0f)) / (2.0f * test_X.shape[0]),
                (X, test_X), (Y, test_Y));
            log.Log(DateTimeOffset.UtcNow, "Information", $"Testing cost={testing_cost}, name={testing_cost.name}");
            var difference = Math.Abs((float)training_cost - (float)testing_cost);
            log.Log(DateTimeOffset.UtcNow, "Information", $"Absolute mean square loss difference: {difference}");

            //var savePath = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel\\MyModel";
            //var saver = tf.train.Saver();
            //saver.save(session, savePath);

            return difference < 0.01;
        }                
    }
}
