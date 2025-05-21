using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFCSV.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.EntityWriter
{
    public class CSVAuditEventWriter : IEntityWriter<AuditEvent>
    {
        private readonly CSVEntityWriter<AuditEvent> _csvWriter = new CSVEntityWriter<AuditEvent>();

        private readonly IAuditEventTypeService _auditEventTypeService;

        public CSVAuditEventWriter(string file, Char delimiter, Encoding encoding,
                            IAuditEventTypeService auditEventTypeService)
        {
            _csvWriter.Delimiter = delimiter;
            _csvWriter.Encoding = encoding;
            _csvWriter.File = file;

            _auditEventTypeService = auditEventTypeService;
        }

        public void Write(IEnumerable<AuditEvent> auditEvents)
        {
            var auditEventTypes = _auditEventTypeService.GetAll();

            _csvWriter.AddColumn("Id", (auditEvent) => auditEvent.Id);
            _csvWriter.AddColumn("CreatedDateTime", (auditEvent) => auditEvent.CreatedDateTime.ToString());
            _csvWriter.AddColumn("AuditEventType", (auditEvent) => auditEventTypes.First(aet => aet.Id == auditEvent.TypeId).Name);

            _csvWriter.Write(auditEvents);
        }
    }
}
