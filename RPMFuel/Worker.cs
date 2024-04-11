using Microsoft.Extensions.Options;

namespace RPMFuel
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<JobConfigOptions> _jobConfig;

        public Worker(ILogger<Worker> logger, IOptions<JobConfigOptions> jobConfig)
        {
            _logger = logger;
            _jobConfig = jobConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                var delay = TimeSpan.FromSeconds(_jobConfig.Value.DelayInSeconds);
                await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
            }
        }
    }
}
