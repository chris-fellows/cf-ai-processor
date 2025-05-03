using CFAIProcessor.Common.Services;
using CFAIProcessor.Constants;
using CFAIProcessor.EntityReader;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Prediction;
using CFAIProcessor.Seed;
using CFAIProcessor.Services;
using CFAIProcessor.SystemTask;
using CFAIProcessor.UI.Components;
using CFAIProcessor.UI.Data;
using CFAIProcessor.UI.Services;
using CFUtilities.Utilities;

var builder = WebApplication.CreateBuilder(args);

var registerSeedDataLoad = true;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");
//Directory.Delete(configFolder, true);

// Add data services
builder.Services.AddScoped<IChartTypeService>((scope) =>
{
    return new XmlChartTypeService(Path.Combine(configFolder, "ChartType"));
});
builder.Services.AddScoped<IUserService>((scope) =>
{
    return new XmlUserService(Path.Combine(configFolder, "User"), scope.GetRequiredService<IPasswordService>());
});

builder.Services.AddScoped<IDataSetInfoService>((scope) =>
{
    return new DataSetInfoService("D:\\Data\\Dev\\C#\\cf-ai-processor-local");  // TODO: Remove this
});

// Add chart data service
// TODO: Support difference classes for different data sources
builder.Services.AddScoped<IChartDataService, CSVChartDataService>();

// Add password service
builder.Services.AddScoped<IPasswordService, PBKDF2PasswordService>();

builder.Services.AddScoped<IPredictionRequestProcessor, LocalPredictionRequestProcessor>();

// Set system task list
builder.Services.AddSingleton<ISystemTaskList>((scope) =>
{    
    // Set system task configs
    var systemTaskConfigs = new List<SystemTaskConfig>()
    {
        //new SystemTaskConfig()
        //{
        //    SystemTaskName = SystemTaskTypeNames.RunPrediction,
        //    ExecuteFrequency = TimeSpan.Zero
        //}        
    };
    systemTaskConfigs.ForEach(stc => stc.NextExecuteTime = DateTimeUtilities.GetNextTaskExecuteTimeFromFrequency(stc.ExecuteFrequency));

    return new SystemTaskList(5, systemTaskConfigs);
});

// Add seed data. Only need it as a one-off
if (registerSeedDataLoad)
{
    builder.Services.AddKeyedScoped<IEntityReader<ChartType>, ChartTypeSeed1>("ChartTypeSeed");
    builder.Services.AddKeyedScoped<IEntityReader<User>, UserSeed1>("UserSeed");    
}

// Add background service for system tasks
builder.Services.AddHostedService<SystemTaskBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//// Load data
//using (var scope = app.Services.CreateScope())
//{   
//    // Enable this to load seed data    
//    new SeedLoader().LoadAsync(scope).Wait();

//    int xxx = 1000;
//}

app.Run();
