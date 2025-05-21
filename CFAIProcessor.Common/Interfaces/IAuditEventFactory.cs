using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IAuditEventFactory
    {
        AuditEvent CreateDataSetInfoAdded(string createdUserId, string dataSetInfoId);

        AuditEvent CreateError(string createdUserId, string errorMessage, List<AuditEventParameter> parameters);

        AuditEvent CreateUserAdded(string createdUserId, string userId);       

        AuditEvent CreatePasswordResetAdded(string createdUserId, string passwordResetId);

        AuditEvent CreatePasswordUpdated(string createdUserId, string userId);

        AuditEvent CreateUserLogInSuccess(string createdUserId, string userId);

        AuditEvent CreateUserLogOut(string createdUserId, string userId);

        AuditEvent CreateUserLogInError(string createdUserId, string username);
    }
}
