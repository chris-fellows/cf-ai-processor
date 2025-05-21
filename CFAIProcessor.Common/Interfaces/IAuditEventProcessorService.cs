using CFAIProcessor.Models;

namespace CFAIProcessor.Interfaces
{
    public interface IAuditEventProcessorService
    {
        Task ProcessAsync(AuditEvent auditEvent);
    }
}
