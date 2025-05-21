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
using CFAIProcessor.UI.Extensions;
using CFAIProcessor.UI.Services;
using CFAIProcessor.UI.Utilities;
using CFUtilities.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var registerSeedDataLoad = true;

// CMF Added
// Set authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "auth_cookie";
            options.LoginPath = "/login";
            options.Cookie.MaxAge = TimeSpan.FromMinutes(120);
            options.AccessDeniedPath = "/access-denied";
        });

builder.Services.AddAuthorization();            // CMF Added
builder.Services.AddCascadingAuthenticationState();     // CMF Added
builder.Services.AddHttpContextAccessor();  // Added for IRequestContextService

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");
//Directory.Delete(configFolder, true);

// Add data services
builder.Services.AddScoped<IAuditEventService>((scope) =>
{
    return new XmlAuditEventService(Path.Combine(configFolder, "AuditEvent"),
                scope.GetRequiredService<IAuditEventProcessorService>());
});
builder.Services.AddScoped<IAuditEventTypeService>((scope) =>
{
    return new XmlAuditEventTypeService(Path.Combine(configFolder, "AuditEventType"));
});
builder.Services.AddScoped<IChartTypeService>((scope) =>
{
    return new XmlChartTypeService(Path.Combine(configFolder, "ChartType"));
});
builder.Services.AddScoped<IPredictionModelService>((scope) =>
{
    return new XmlPredictionModelService(Path.Combine(configFolder, "PredictionModel"));
});
builder.Services.AddScoped<ISystemValueTypeService>((scope) =>
{
    return new XmlSystemValueTypeService(Path.Combine(configFolder, "SystemValueType"));
});
builder.Services.AddScoped<IUserService>((scope) =>
{
    return new XmlUserService(Path.Combine(configFolder, "User"), scope.GetRequiredService<IPasswordService>());
});

builder.Services.AddScoped<IDataSetInfoService>((scope) =>
{
    return new DataSetInfoService(ConfigUtilities.DataSetLocalFolder);  // TODO: Remove this    
});

// Add toast service
builder.Services.AddSingleton<IToastService, ToastService>();

builder.Services.AddScoped<IAuditEventFactory, AuditEventFactory>();

builder.Services.AddScoped<IAuditEventProcessorService, AuditEventProcessorService>();

builder.Services.AddScoped<IRequestContextService, RequestContextService>();

// Add chart data service
// TODO: Support difference classes for different data sources
builder.Services.AddScoped<IChartDataService, CSVChartDataService>();

// Add password service
builder.Services.AddScoped<IPasswordService, PBKDF2PasswordService>();

builder.Services.AddScoped<IPredictionRequestProcessor, LocalPredictionRequestProcessor>();

builder.Services.RegisterAllTypes<ISystemTask>(new[] { typeof(Program).Assembly, typeof(ISystemTask).Assembly });

// Set system task list
builder.Services.AddSingleton<ISystemTaskList>((scope) =>
{    
    // Set system task configs
    var systemTaskConfigs = new List<SystemTaskConfig>()
    {
        new SystemTaskConfig()
        {
            SystemTaskName = SystemTaskTypeNames.RunPredictionPredict,
            ExecuteFrequency = TimeSpan.Zero        // Only runs on demand
        },
        new SystemTaskConfig()
        {
            SystemTaskName = SystemTaskTypeNames.RunPredictionTrain,
            ExecuteFrequency = TimeSpan.Zero        // Only runs on demand
        }     

        //new SystemTaskConfig()
        //{
        //    SystemTaskName = SystemTaskTypeNames.RunPrediction,
        //    ExecuteFrequency = TimeSpan.Zero
        //}        
    };
    systemTaskConfigs.ForEach(stc => stc.NextExecuteTime = stc.ExecuteFrequency== TimeSpan.Zero ? DateTimeOffset.MaxValue :
                            DateTimeUtilities.GetNextTaskExecuteTimeFromFrequency(stc.ExecuteFrequency));

    return new SystemTaskList(5, systemTaskConfigs);
});

// Add seed data. Only need it as a one-off
if (registerSeedDataLoad)
{
    builder.Services.AddKeyedScoped<IEntityReader<AuditEventType>, AuditEventTypeSeed1>("AuditEventTypeSeed");
    builder.Services.AddKeyedScoped<IEntityReader<ChartType>, ChartTypeSeed1>("ChartTypeSeed");
    builder.Services.AddKeyedScoped<IEntityReader<User>, UserSeed1>("UserSeed");
    builder.Services.AddKeyedScoped<IEntityReader<SystemValueType>, SystemValueTypeSeed1>("SystemValueTypeSeed");
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
