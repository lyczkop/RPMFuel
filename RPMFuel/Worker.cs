using Microsoft.Extensions.Options;
using RPMFuel.Domain;
using RPMFuel.Domain.Models.Configs;

namespace RPMFuel;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly WorkerConfigOptions _workerConfig;
    private readonly PetrolService _petrolService;

    public Worker(ILogger<Worker> logger, IOptions<WorkerConfigOptions> options, PetrolService petrolService)
    {
        _logger = logger;
        _workerConfig = options.Value;
        _petrolService = petrolService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            _logger.LogInformation("Worker running at: {time} with {interval} seconds interval",
                DateTimeOffset.Now, _workerConfig.DelayInSeconds);

            await _petrolService.UpdatePrices();

            var delay = TimeSpan.FromSeconds(_workerConfig.DelayInSeconds);
            await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
        }
    }
}
