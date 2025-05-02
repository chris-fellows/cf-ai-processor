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

namespace CFAIProcessor.Prediction
{
    internal class PredictionProcessorSaved<TEntityType>
    {

        public void Run(PredictionConfig predictionConfig, ISimpleLog log)
        {
            tf.compat.v1.disable_eager_execution();

            //var model = tf.keras.models.load_model(predictionConfig.ModelFolder);
            var modelTest = tf.saved_model.load(predictionConfig.ModelFolder + "\\");            
            
            

            var savePath = $"D:\\Data\\Dev\\C#\\cf-ai-processor\\CFAIProcessor.UI\\bin\\Debug\\net8.0-windows\\PredictionModel\\"; 
                       

            // https://cv-tricks.com/tensorflow-tutorial/save-restore-tensorflow-models-quick-complete-tutorial/
            using var session = tf.Session();               
            var saver = tf.train.import_meta_graph(Path.Combine(savePath, "DefaultModel.meta"));                        

            saver.restore(session, tf.train.latest_checkpoint(savePath));

            //saver.restore(session, tf.train.latest_checkpoint("./"));

            var X = session.graph.get_tensor_by_name("placeholderX:0");
            var Y = session.graph.get_tensor_by_name("placeholderY:0");

            var W = session.graph.get_tensor_by_name("weight:0");            
            var b = session.graph.get_tensor_by_name("bias:0");         

            //var result1 = session.run(W);

            //var optimizer = session.graph.get_operation_by_name("GradientDescent");            

            var pred = session.graph.get_tensor_by_name("pred:0");            
                        
            // Testing example
            var test_X = np.array(6.83f, 4.668f, 8.9f, 7.91f, 5.7f, 8.7f, 3.1f, 2.1f);
            var test_Y = np.array(1.84f, 2.273f, 3.2f, 2.831f, 2.92f, 3.24f, 1.35f, 1.03f);
            log.Log(DateTimeOffset.UtcNow, "Information", "Testing... (Mean square loss Comparison)");
            var testing_cost = session.run(tf.reduce_sum(tf.pow(pred - Y, 2.0f)) / (2.0f * test_X.shape[0]),
                (X, test_X), (Y, test_Y));
            log.Log(DateTimeOffset.UtcNow, "Information", $"Testing cost={testing_cost}, name={testing_cost.name}");
            //var difference = Math.Abs((float)training_cost - (float)testing_cost);
            //log.Log(DateTimeOffset.UtcNow, "Information", $"Absolute mean square loss difference: {difference}");            

            int xxx = 1000;
        }
    }
}
