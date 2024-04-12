using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPMFuel.Config;

namespace RPMFuel.HttpClients;

public class EIAClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<EIAClientConfigOptions> _options;

    public EIAClient(HttpClient httpClient, IOptions<EIAClientConfigOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<EIAResponse> GetCatalogItems()
    {
        var uri = $"{_options.Value.BaseApiUrl}{_options.Value.PetrolPath}{_options.Value.ApiKey}";
        var responseString = await _httpClient.GetStringAsync(uri);
        var response = JsonConvert.DeserializeObject<EIAResponse>(responseString);

        return response;
    }
}
