using CFAIProcessor.Constants;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Prediction;
using CFAIProcessor.SystemTask;
using CFAIProcessor.UI.Components;
using CFAIProcessor.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IPredictionRequestProcessor, LocalPredictionRequestProcessor>();

// Set system task list
builder.Services.AddSingleton<ISystemTaskList>((scope) =>
{    
    // Set system task configs
    var systemTaskConfigs = new List<SystemTaskConfig>()
    {
        new SystemTaskConfig()
        {
            SystemTaskName = SystemTaskTypeNames.RunPrediction,
            ExecuteFrequency = TimeSpan.Zero
        }        
    };
    systemTaskConfigs.ForEach(stc => stc.NextExecuteTime = DateTimeUtilities.GetNextTaskExecuteTimeFromFrequency(stc.ExecuteFrequency));

    return new SystemTaskList(5, systemTaskConfigs);
});

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

app.Run();
