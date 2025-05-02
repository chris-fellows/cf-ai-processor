using CFAIProcessor.Models;

namespace CFAIProcessor.Interfaces
{
    /// <summary>
    /// Processes prediction request
    /// </summary>
    public interface IPredictionRequestProcessor
    {
        PredictionResponse Run(PredictionRequest predictionRequest, CancellationToken cancellationToken);
    }
}
