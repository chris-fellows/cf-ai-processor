using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;

namespace CFAIProcessor.Services
{
    public class XmlSystemValueTypeService : XmlEntityWithIdAndNameService<SystemValueType, string>, ISystemValueTypeService
    {
        public XmlSystemValueTypeService(string folder) : base(folder,
                                                "SystemValueType.*.xml",
                                              (systemValueType) => $"SystemValueType.{systemValueType.Id}.xml",
                                                (systemValueTypeId) => $"SystemValueType.{systemValueTypeId}.xml",
                                                (systemValueType) => systemValueType.Name)
        {

        }
    }
}
