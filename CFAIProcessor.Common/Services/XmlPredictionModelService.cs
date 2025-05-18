using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;

namespace CFAIProcessor.Services
{
    public class XmlPredictionModelService : XmlEntityWithIdAndNameService<PredictionModel, string>, IPredictionModelService
    {        
        public XmlPredictionModelService(string folder) : base(folder,
                                                "PredictionModel.*.xml",
                                              (predictionModel) => $"PredictionModel.{predictionModel.Id}.xml",
                                                (predictionModelId) => $"PredictionModel.{predictionModelId}.xml",
                                                (predictionModel) => predictionModel.Name)
        {
            
        }     
    }
}
