using Microsoft.Extensions.Options;
using RPMFuel.Config;

namespace RPMFuel;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<WorkerConfigOptions> _options;
    private readonly PetrolService _petrolService;

    public Worker(ILogger<Worker> logger, IOptions<WorkerConfigOptions> options, PetrolService petrolService)
    {
        _logger = logger;
        _options = options;
        _petrolService = petrolService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _petrolService.UpdatePrices();

            var delay = TimeSpan.FromSeconds(_options.Value.DelayInSeconds);
            await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
        }
    }
}
