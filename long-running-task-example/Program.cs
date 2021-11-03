using long_running_task_example.BackgroundTask;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "long_running_task_example", Version = "v1" });
});

builder.Services.AddLogging();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<BackgroundQueueHostedService>();
builder.Services.AddScoped<ILongRunningWork, LongRunningWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "long_running_task_example v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();

