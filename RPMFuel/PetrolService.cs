using Microsoft.Extensions.Options;
using RPMFuel.Infrastructure.Config;
using RPMFuel.Infrastructure.Database;
using RPMFuel.Infrastructure.Database.Entities;
using RPMFuel.Infrastructure.HttpClients;

namespace RPMFuel;

public class PetrolService
{
    private readonly ILogger<PetrolService> _logger;
    private readonly EIAClient _eIAClient;
    private readonly FuelRepository _fuelRepository;
    private readonly WorkerConfigOptions _workerConfig;

    public PetrolService(
        ILogger<PetrolService> logger,
        EIAClient eIAClient,
        FuelRepository fuelRepository,
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
        var res = await _eIAClient.GetPetrolData();

        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var arrivedFuelDataEntities = res.Data.Where(d => now.DayNumber - d.Period.DayNumber < _workerConfig.DaysBehind)
            .Select(d => new FuelDataEntity(d.Period, d.Value, d.Units))
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
