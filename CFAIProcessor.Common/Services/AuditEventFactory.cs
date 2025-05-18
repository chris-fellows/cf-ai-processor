using CFAIProcessor.Constants;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class AuditEventFactory : IAuditEventFactory
    {
        protected readonly IAuditEventService _auditEventService;
        protected readonly IAuditEventTypeService _auditEventTypeService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public AuditEventFactory(IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,
                        ISystemValueTypeService systemValueTypeService)
        {
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _systemValueTypeService = systemValueTypeService;
        }

        public AuditEvent CreateDataSetInfoAdded(string createdUserId, string dataSetInfoId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.DataSetInfoAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.DataSetInfoId).Id,
                        Value = dataSetInfoId
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateError(string createdUserId, string errorMessage, List<AuditEventParameter> parameters)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.Error);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.ErrorMessage).Id,
                        Value = errorMessage
                    },
                }
            };
            auditEvent.Parameters.AddRange(parameters);

            return auditEvent;
        }
   
        public AuditEvent CreateUserAdded(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.UserAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.UserId).Id,
                        Value = userId
                    }
                }
            };

            return auditEvent;
        }
    
        public AuditEvent CreatePasswordResetAdded(string createdUserId, string passwordResetId)
        {
            //var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.PasswordResetCreated);
            //var systemValueTypes = _systemValueTypeService.GetAll();

            //var auditEvent = new AuditEvent()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    TypeId = auditEventType.Id,
            //    CreatedDateTime = DateTimeOffset.UtcNow,
            //    CreatedUserId = createdUserId,
            //    Parameters = new List<AuditEventParameter>()
            //    {
            //        new AuditEventParameter()
            //        {
            //            SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_PasswordResetId).Id,
            //            Value = passwordResetId
            //        }
            //    }
            //};

            //return auditEvent;
            return new();
        }

        public AuditEvent CreateUserLogInSuccess(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.UserLogInSuccess);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.UserId).Id,
                        Value = userId
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateUserLogOut(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.UserLogOut);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.UserId).Id,
                        Value = userId
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateUserLogInError(string createdUserId, string username)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.UserLogInError);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.Notes).Id,
                        Value = username
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreatePasswordUpdated(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.Name == AuditEventTypeNames.PasswordUpdated);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.Name == SystemValueTypeNames.UserId).Id,
                        Value = userId
                    }
                }
            };

            return auditEvent;
        }
    }
}
