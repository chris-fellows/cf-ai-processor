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
            var chartTypeService = scope.ServiceProvider.GetRequiredService<IChartTypeService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Get seed data services
            var chartTypeSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<ChartType>>("ChartTypeSeed");
            var userSeed = scope.ServiceProvider.GetRequiredKeyedService<IEntityReader<User>>("UserSeed");

            // Check that no data exists
            var chartTypesOld = await chartTypeService.GetAllAsync();
            if (chartTypesOld.Any())
            {
                throw new ArgumentException("Cannot load seed data because data already exists");
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
        }
    }
}
