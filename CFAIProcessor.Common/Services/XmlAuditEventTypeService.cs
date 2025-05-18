using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;

namespace CFAIProcessor.Services
{
    public class XmlAuditEventTypeService : XmlEntityWithIdAndNameService<AuditEventType, string>, IAuditEventTypeService
    {
        public XmlAuditEventTypeService(string folder) : base(folder,
                                                "AuditEventType.*.xml",
                                              (auditEventType) => $"AuditEventType.{auditEventType.Id}.xml",
                                                (auditEventTypeId) => $"AuditEventType.{auditEventTypeId}.xml",
                                                (auditEventType) => auditEventType.Name)
        {

        }
    }
}
