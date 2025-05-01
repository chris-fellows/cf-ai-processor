using Tensorflow.NumPy;

namespace CFAIProcessor.Interfaces
{
    /// <summary>
    /// Data source for prediction
    /// </summary>
    internal interface IPredictionDataSource
    {
        NDArray GetFeatures(string dataFile, bool normalise);

        NDArray GetLabels(string dataFile, bool normalise);
    }
}
