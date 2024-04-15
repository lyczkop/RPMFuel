using System.ComponentModel.DataAnnotations;

namespace RPMFuel.Domain.Models.Configs;

public class EIAClientConfigOptions
{
    public const string Name = "EIAClientConfigOptions";

    [Required]
    public required string BaseApiUrl { get; set; }

    [Required]
    public required string PetrolPath { get; set; }

    [Required]
    public required string ApiKey { get; set; }
}
