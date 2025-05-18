using CFAIProcessor.EntityReader;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;

namespace CFAIProcessor.UI.Data
{
    /// <summary>
    /// Loads seed data
    /// </summary>
    public class SeedLoader
    {
        public async Task LoadAsync(IServiceScope scope)
        {
            // Get services
            var auditEventFactory = scope.ServiceProvider.GetRequiredService<IAuditEventFactory>();
            var auditEventService = scope.ServiceProvider.GetRequiredService<IAuditEventService>();
            var auditEventTypeService = scope.ServiceProvider.GetRequiredService<IAuditEventTypeService>();
            var chartTypeService = scope.ServiceProvider.GetRequiredService<IChartTypeService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();

            // Get seed data services
            var auditEventTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<AuditEventType>>("AuditEventTypeSeed");
            var chartTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<ChartType>>("ChartTypeSeed");
            var userSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<User>>("UserSeed");
            var systemValueTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<SystemValueType>>("SystemValueTypeSeed");

            // Check that no data exists
            var chartTypesOld = await chartTypeService.GetAllAsync();
            if (chartTypesOld.Any())
            {
                throw new ArgumentException("Cannot load seed data because data already exists");
            }

            // Add audit event types
            var auditEventTypesNew = auditEventTypeSeed.Read();
            foreach (var auditEventType in auditEventTypesNew)
            {
                await auditEventTypeService.AddAsync(auditEventType);
            }

            // Add system value types
            var systemValueTypesNew = systemValueTypeSeed.Read();
            foreach (var systemValueType in systemValueTypesNew)
            {
                await systemValueTypeService.AddAsync(systemValueType);
            }

            // Add chart types
            var chartTypesNew = chartTypeSeed.Read();
            foreach (var chartType in chartTypesNew)
            {
                await chartTypeService.AddAsync(chartType);
            }

            // Add users
            var usersNew = userSeed.Read();
            foreach (var user in usersNew)
            {
                await userService.AddAsync(user);                
            }

            // Add 'User added' audit event
            var users = await userService.GetAllAsync();
            var systemUser = users.First(u => u.GetUserType() == Enums.UserTypes.System);
            foreach(var user in users)
            {
                var auditEvent = auditEventFactory.CreateUserAdded(systemUser.Id, user.Id);
                await auditEventService.AddAsync(auditEvent);
            }
        }
    }
}
