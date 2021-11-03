using long_running_task_example.BackgroundTask;
using Microsoft.AspNetCore.Mvc;

namespace long_running_task_example.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _logger = logger;
        _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task<IActionResult> LongRunningWork([FromBody] string data)
    {
        //Save in db unique identifier with track de work
        var id = Guid.NewGuid();
        _backgroundTaskQueue.EnqueueTask(async (serviceScopeFactory, cancellationToken) =>
        {
            // Get services
            using var scope = serviceScopeFactory.CreateScope();
            var myService = scope.ServiceProvider.GetRequiredService<ILongRunningWork>();

            try
            {
                // Do something expensive
                await myService.Run(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not do something expensive");
            }
        });
        return new AcceptedResult(id.ToString(), null);
    }
}

