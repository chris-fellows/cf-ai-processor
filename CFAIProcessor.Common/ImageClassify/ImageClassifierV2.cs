using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using Tensorflow;
using Tensorflow.Keras;
using Tensorflow.Keras.Callbacks;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Losses;
using Tensorflow.Keras.Utils;
using Tensorflow.NumPy;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;

namespace CFAIProcessor.ImageClassify
{
    public class ImageClassifierV2
    {
        int batch_size = 32;
        //int epochs = 10;
        //Shape img_dim = (64, 64);     // Original
        //IDatasetV2 train_ds, val_ds;
        //Model model;

        Shape _imageDimensions = (224, 224);

        private Sequential GetInitialModel(int classCount)
        {
            //var classCount = imageClassifyConfig.ClassNames.Count;
            var layers = keras.layers;
            var modelLayers = new List<ILayer>
            {
                layers.Rescaling(1.0f / 255, input_shape: (_imageDimensions.dims[0], _imageDimensions.dims[1], 3)),
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

        private static string DecompressImageSet(string dataSource, string imageFolder)
        {
            // Unzip files
            using (var archive = ZipFile.Open(dataSource, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(imageFolder);
                archive.Dispose();
            }

            return imageFolder; // TODO: Return folder that contains class name folders
        }

        private static List<string> GetClassNamesFromImageFolder(string folder)
        {
            var classNames = new List<string>();

            foreach(var directory in  Directory.GetDirectories(folder))
            {
                classNames.Add(new DirectoryInfo(directory).Name);
            }

            return classNames;
        }

        public ImageClassifyModel Train(ImageTrainConfig imageTrainConfig,
                         CancellationToken cancellationToken)
        {
            var imageClassifyModel = new ImageClassifyModel()
            {
                Id = Guid.NewGuid().ToString(),
                DataSetInfoId = imageTrainConfig.TrainImageSetInfoId,                
                CreatedUserId = imageTrainConfig.UserId,
                ModelFolder = imageTrainConfig.ModelFolder,
                Name = imageTrainConfig.ModelName,
                TrainConfig = imageTrainConfig
            };

            // Decompress train images
            var trainImageRootFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var trainImageFolder = DecompressImageSet(imageTrainConfig.TrainImageSetInfoDataSource, trainImageRootFolder);

            if (cancellationToken.IsCancellationRequested) return imageClassifyModel;

            // Decompress valid images
            var validImageRootFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var validImageFolder = DecompressImageSet(imageTrainConfig.ValidImageSetInfoDataSource, validImageRootFolder);

            if (cancellationToken.IsCancellationRequested) return imageClassifyModel;

            // Get class names
            var classNames = GetClassNamesFromImageFolder(trainImageFolder);

            // Get model
            var model = GetInitialModel(classNames.Count);

            // Compile model
            model.compile(optimizer: keras.optimizers.Adam(),
                            loss: keras.losses.SparseCategoricalCrossentropy(from_logits: true),
                            metrics: new[] { "accuracy" });         

            // https://www.tensorflow.org/tutorials/images/classification
            // Get training images        
            var trainImages = keras.preprocessing.image_dataset_from_directory(trainImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                   
                                    subset: "training",
                                    image_size: _imageDimensions,
                                    batch_size: 10);
            var classNames1 = trainImages.class_names;

            // Get validation images
            var validImages = keras.preprocessing.image_dataset_from_directory(validImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                    
                                    subset: "validation",
                                    image_size: _imageDimensions,
                                    batch_size: 10);

            //// Get test images
            //var testImages = keras.preprocessing.image_dataset_from_directory(imageClassifyConfig.TestImageFolder,
            //                        color_mode: "rgb",
            //                        labels: "inferred",     // From directory structure                                    
            //                                                //label_mode: "int",
            //                        image_size: _imageDimensions,
            //                        batch_size: 10);

            foreach (var (images, labels) in trainImages)
            {
                int xxxxxxx = 1000;
            }

            // Train
            var trainResult = model.fit(trainImages, validation_data: validImages, epochs: imageTrainConfig.TrainEpochs, verbose: 2);  // Added verbose param           

            // Save model
            if (!String.IsNullOrEmpty(imageTrainConfig.ModelFolder))
            {
                Directory.CreateDirectory(imageTrainConfig.ModelFolder);
                model.save(imageTrainConfig.ModelFolder);
            }

            return imageClassifyModel;
        }

        public void Classify(ImageClassifyConfig imageClassifyConfig,
                            CancellationToken cancellationToken)
        {
            if (!Directory.Exists(imageClassifyConfig.ModelFolder))
            {
                throw new DirectoryNotFoundException($"Model folder {imageClassifyConfig.ModelFolder} does not exist");
            }

            // Load model
            var model = tf.keras.models.load_model(imageClassifyConfig.ModelFolder);

            var testImageRootFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var testImageFolder = DecompressImageSet(imageClassifyConfig.TestImageSetInfoDataSource, testImageRootFolder);

            // Get test images
            var testImages = keras.preprocessing.image_dataset_from_directory(testImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                                            
                                    image_size: _imageDimensions,
                                    batch_size: 10);

            // Predict
            var predictions = model.predict(testImages);
            var score = tf.nn.softmax(predictions[0]);
        }

        public void TrainAndClassify(ImageClassifyConfigV1 imageClassifyConfig)
        {            
            // Get model
            var model = GetInitialModel(imageClassifyConfig.ClassNames.Count);

            // Compile model
            model.compile(optimizer: keras.optimizers.Adam(),
                            loss: keras.losses.SparseCategoricalCrossentropy(from_logits: true),
                            metrics: new[] { "accuracy" });

            // Prepare images
            //PrepareImages(imageClassifyConfig);

            // https://www.tensorflow.org/tutorials/images/classification
            // Get training images        
            var trainImages = keras.preprocessing.image_dataset_from_directory(Path.Combine(imageClassifyConfig.TrainImageFolder),
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                   
                                    subset: "training",
                                    image_size: _imageDimensions,
                                    batch_size: 10);
            var classNames1 = trainImages.class_names;

            // Get validation images
            var validImages = keras.preprocessing.image_dataset_from_directory(imageClassifyConfig.ValidImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                    
                                    subset: "validation",
                                    image_size: _imageDimensions,
                                    batch_size: 10);

            // Get test images
            var testImages = keras.preprocessing.image_dataset_from_directory(imageClassifyConfig.TestImageFolder,
                                    color_mode: "rgb",
                                    labels: "inferred",     // From directory structure                                    
                                                            //label_mode: "int",
                                    image_size: _imageDimensions,
                                    batch_size: 10);

            foreach (var (images, labels) in testImages)
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
                foreach (var file in Directory.GetFiles(imageFolder, "*", SearchOption.AllDirectories))
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
