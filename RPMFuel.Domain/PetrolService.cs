using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RPMFuel.Domain.Interfaces;
using RPMFuel.Domain.Models.Configs;

namespace RPMFuel.Domain;

public class PetrolService
{
    private readonly ILogger<PetrolService> _logger;
    private readonly IEIAClient _eIAClient;
    private readonly IFuelRepository _fuelRepository;
    private readonly WorkerConfigOptions _workerConfig;

    public PetrolService(
        ILogger<PetrolService> logger,
        IEIAClient eIAClient,
        IFuelRepository fuelRepository,
        IOptions<WorkerConfigOptions> options)
    {
        _logger = logger;
        _eIAClient = eIAClient;
        _fuelRepository = fuelRepository;
        _workerConfig = options.Value;
    }

    public async Task UpdatePrices()
    {
        _logger.LogInformation("{MethodName)} started. Look {DaysBehind} days behind",
            nameof(UpdatePrices), _workerConfig.DaysBehind);

        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var arrivedFuelDataEntities = (await _eIAClient.GetPetrolData())
            .Where(d => now.DayNumber - d.Period.DayNumber < _workerConfig.DaysBehind)
            .ToList();

        var currentFuelPeriods = (await _fuelRepository.GetAllAsync())
            .Select(e => e.Period);

        var fuelDataToAdd = arrivedFuelDataEntities
                    .Where(e => !currentFuelPeriods.Contains(e.Period))
                    .ToList();

        await _fuelRepository.AddManyAsync(fuelDataToAdd);
        _logger.LogInformation("{MethodName}: Added {count} new records",
            nameof(UpdatePrices), fuelDataToAdd.Count);
        _logger.LogInformation("{MethodName} end", nameof(UpdatePrices));
    }
}
