using System.Collections.Generic;
using Tensorflow;
using Tensorflow.Keras;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow.Keras.Utils;
using System.IO;
using Tensorflow.Keras.Engine;
using CFAIProcessor.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using Tensorflow.NumPy;
using Tensorflow.Keras.Callbacks;
using Tensorflow.Keras.Losses;

namespace CFAIProcessor.ImageClassify
{
    public class ImageClassifierV1
    {
        int batch_size = 32;
        //int epochs = 10;
        //Shape img_dim = (64, 64);     // Original
        //IDatasetV2 train_ds, val_ds;
        //Model model;

        Shape img_dim = (224, 224);

        private Sequential GetModel(ImageClassifyConfigV1 imageClassifyConfig)
        {
            var classCount = imageClassifyConfig.ClassNames.Count;
            var layers = keras.layers;
            var modelLayers = new List<ILayer>
            {
                layers.Rescaling(1.0f / 255, input_shape: (img_dim.dims[0], img_dim.dims[1], 3)),
                layers.Conv2D(16, 3, padding: "same", activation: keras.activations.Relu),
                layers.MaxPooling2D(),
                /*layers.Conv2D(32, 3, padding: "same", activation: "relu"),
                layers.MaxPooling2D(),
                layers.Conv2D(64, 3, padding: "same", activation: "relu"),
                layers.MaxPooling2D(),*/
                layers.Flatten(),
                layers.Dense(128, activation: keras.activations.Relu),
                layers.Dense(classCount)
            };
            var model = keras.Sequential(modelLayers);
            return model;
        }

        public void TrainAndClassify(ImageClassifyConfigV1 imageClassifyConfig)
        {
            var classCount = imageClassifyConfig.ClassNames.Count;

            var model = GetModel(imageClassifyConfig);
            
            //var layers = keras.layers;
            //var modelLayers = new List<ILayer>
            //{
            //    layers.Rescaling(1.0f / 255, input_shape: (img_dim.dims[0], img_dim.dims[1], 3)),
            //    layers.Conv2D(16, 3, padding: "same", activation: keras.activations.Relu),
            //    layers.MaxPooling2D(),
            //    /*layers.Conv2D(32, 3, padding: "same", activation: "relu"),
            //    layers.MaxPooling2D(),
            //    layers.Conv2D(64, 3, padding: "same", activation: "relu"),
            //    layers.MaxPooling2D(),*/
            //    layers.Flatten(),
            //    layers.Dense(128, activation: keras.activations.Relu),
            //    layers.Dense(classCount)
            //};
            //var model = keras.Sequential(modelLayers);
            
            // Compile model
            model.compile(optimizer: keras.optimizers.Adam(),
                            loss: keras.losses.SparseCategoricalCrossentropy(from_logits: true),
                            metrics: new[] { "accuracy" });

            // Prepare images
            //PrepareImages(imageClassifyConfig);

            // https://www.tensorflow.org/tutorials/images/classification
            // Get training images
            if (!Directory.Exists(imageClassifyConfig.TrainImageFolder))
            {
                int zzz = 1000;
            }
            var trainImages = keras.preprocessing.image_dataset_from_directory(Path.Combine(imageClassifyConfig.TrainImageFolder),
                
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                   
                                    subset: "training",                                                                       
                                    image_size: img_dim,
                                    batch_size: 10);
            var classNames1 = trainImages.class_names;
    
            // Get validation images
            var validImages = keras.preprocessing.image_dataset_from_directory(imageClassifyConfig.ValidImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                    
                                    subset: "validation",
                                    image_size: img_dim,
                                    batch_size: 10);

            // Get test images
            var testImages = keras.preprocessing.image_dataset_from_directory(imageClassifyConfig.TestImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                    
                                    //label_mode: "int",
                                    image_size: img_dim,
                                    batch_size: 10);

            foreach(var (images, labels) in testImages)
            {
                int xxxxxxx = 1000;
            }

            var testClassNames = testImages.class_names;            

            // Train
            var trainResult = model.fit(trainImages, validation_data: validImages, epochs: imageClassifyConfig.TrainEpochs, verbose: 2);  // Added verbose param            

            /*
            var acc = trainResult.history["accuracy"];
            var val_acc = trainResult.history["val_accuracy"];

            var loss = trainResult.history["loss"];
            var val_loss = trainResult.history["val_loss"];
            */

            // Predict
            var predictions = model.predict(testImages);
            var score = tf.nn.softmax(predictions[0]);                        

            /*
            print(
            "This image most likely belongs to {} with a {:.2f} percent confidence."
            .format(class_names[np.argmax(score)], 100 * np.max(score))
            )
            */

            int xxx = 1000;
        }   
        
        private static bool IsValidImageFile(string file)
        {
            if (String.IsNullOrEmpty(file) || !File.Exists(file)) return false;

            try
            {
                var image = Image.FromFile(file);
                if (image != null)
                {
                    image.Dispose();
                    return true;
                }                                
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Prepare images. Copies random images from source to folders for training, validate and test
        /// </summary>
        /// <param name="imageClassifyConfig"></param>
        private void PrepareImages(ImageClassifyConfigV1 imageClassifyConfig)
        {         
            // Set number of images
            var configs = new List<Tuple<string, int>>
            {
                new Tuple<string, int>(imageClassifyConfig.TrainImageFolder, imageClassifyConfig.TrainImageCount),
                new Tuple<string, int>(imageClassifyConfig.ValidImageFolder, imageClassifyConfig.ValidImageCount),
                new Tuple<string, int>(imageClassifyConfig.TestImageFolder, imageClassifyConfig.TestImageCount)                
            };

            var random = new Random();

            var filesUsed = new HashSet<string>();            
            foreach (var config in configs)
            {
                var imageFolder = config.Item1;
                var imageCount = config.Item2;
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                // Clear folder
                foreach(var file in Directory.GetFiles(imageFolder, "*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
                
                // Process each class and copy N random images from source to destination folder
                foreach (var className in imageClassifyConfig.ClassNames)       // Cat/Dog
                {
                    // Get all images for class
                    var allImagesForClass = Directory.GetFiles(Path.Combine(imageClassifyConfig.AllImageFolder, className), "*");

                    // Set destination folder
                    var destinationFolder = Path.Combine(imageFolder, className);                    
                    Directory.CreateDirectory(destinationFolder);

                    for (int index = 0; index < imageCount; index++)
                    {
                        // Select random image
                        var sourceFile = "";
                        do
                        {
                            sourceFile = allImagesForClass[random.Next(0, allImagesForClass.Length - 1)];

                            if (!IsValidImageFile(sourceFile) ||
                                filesUsed.Contains(sourceFile))
                            {
                                sourceFile = "";
                            }
                        } while (String.IsNullOrEmpty(sourceFile));                                   
                        filesUsed.Add(sourceFile);  // So that we don't select again

                        // Copy file
                        var destinationFile = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));                        
                        File.Copy(sourceFile, destinationFile);
                    }
                }
            }

            int xxx = 1000;
        }
    }
}
