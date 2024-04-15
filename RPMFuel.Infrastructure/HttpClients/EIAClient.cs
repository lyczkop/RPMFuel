using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPMFuel.Domain.Interfaces;
using RPMFuel.Domain.Models;
using RPMFuel.Domain.Models.Configs;

namespace RPMFuel.Infrastructure.HttpClients;

public class EIAClient : IEIAClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<EIAClientConfigOptions> _options;

    public EIAClient(HttpClient httpClient, IOptions<EIAClientConfigOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<ICollection<FuelDto>> GetPetrolDataAsync(CancellationToken cancellationToken)
    {
        var uri = $"{_options.Value.BaseApiUrl}{_options.Value.PetrolPath}{_options.Value.ApiKey}";
        var responseString = await _httpClient.GetStringAsync(uri, cancellationToken);
        var response = JsonConvert.DeserializeObject<EIAResponse>(responseString);

        // TODO lyko check null reference

        return response.Response.Data
            .Select(d => new FuelDto(d.Period, d.Value, d.Units))
            .ToList();
    }
}
