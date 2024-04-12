using RPMFuel.HttpClients;

namespace RPMFuel;

public class PetrolService
{
    private readonly ILogger<PetrolService> _logger;
    private readonly EIAClient _eIAClient;

    public PetrolService(ILogger<PetrolService> logger, EIAClient eIAClient)
    {
        _logger = logger;
        _eIAClient = eIAClient;
    }

    public async Task UpdatePrices()
    {
        _logger.LogInformation("UpdatePrices RUN");
        var res = await _eIAClient.GetCatalogItems();
        _logger.LogInformation($"Response: {res}");
    }
}
