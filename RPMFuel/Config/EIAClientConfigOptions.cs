namespace RPMFuel.Config;

public class EIAClientConfigOptions
{
    public const string Name = "EIAClientConfigOptions";
    public required string BaseApiUrl { get; set; }
    public required string PetrolPath { get; set; }
    public required string ApiKey { get; set; }
}
