using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class XmlAuditEventService : XmlEntityWithIdService<AuditEvent, string>, IAuditEventService
    {
        private readonly IAuditEventProcessorService _auditEventProcessorService;

        public XmlAuditEventService(string folder,
            IAuditEventProcessorService auditEventProcessorService) : base(folder,
                                                "AuditEvent.*.xml",
                                              (auditEvent) => $"AuditEvent.{auditEvent.Id}.xml",
                                                (auditEventId) => $"AuditEvent.{auditEventId}.xml")
        {
            _auditEventProcessorService = auditEventProcessorService;
        }

        public List<AuditEvent> GetByFilter(AuditEventFilter filter)
        {
            var auditEvents = GetAll()
                        .Where(i =>
                        (
                           (
                               filter.CreatedDateTimeFrom == null ||
                               i.CreatedDateTime >= filter.CreatedDateTimeFrom
                           ) &&
                           (
                               filter.CreatedDateTimeTo == null ||
                               i.CreatedDateTime <= filter.CreatedDateTimeTo
                           ) &&
                           (
                               filter.AuditEventTypeIds == null ||
                               !filter.AuditEventTypeIds.Any() ||
                               filter.AuditEventTypeIds.Contains(i.TypeId)
                           )
                        )).ToList();

            return auditEvents;
        }
    }
}
