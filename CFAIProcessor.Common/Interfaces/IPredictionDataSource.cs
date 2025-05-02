using PureHDF;
using Tensorflow.NumPy;

namespace CFAIProcessor.Interfaces
{
    /// <summary>
    /// Data source for prediction
    /// </summary>
    internal interface IPredictionDataSource
    {
        /// <summary>
        /// Gets feature values, normalises if required
        /// </summary>
        /// <param name="normalise"></param>
        /// <returns></returns>
        NDArray GetFeatureValues(bool normalise);

        /// <summary>
        /// Gets feature names
        /// </summary>
        string[] FeatureNames { get; }

        /// <summary>
        /// Get non-normalised (original) feature value from normalised value
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        float GetNonNormalisedFeatureValue(int columnIndex, float value);

        /// <summary>
        /// Get label values, normalises if required
        /// </summary>
        /// <param name="normalise"></param>
        /// <returns></returns>
        NDArray GetLabelValues(bool normalise);

        /// <summary>
        /// Get label names
        /// </summary>
        string[] LabelNames { get; }
        
        /// <summary>
        /// Get non-normalised (original) label value from normalised value
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        float GetNonNormalisedLabelValue(int columnIndex, float value);
    }
}
