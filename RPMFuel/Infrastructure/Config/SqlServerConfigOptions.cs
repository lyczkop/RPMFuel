using System.ComponentModel.DataAnnotations;

namespace RPMFuel.Infrastructure.Config;

public class SqlServerConfigOptions
{
    public const string Name = "SqlServerConfigOptions";

    [Required]
    public required string ConnectionString { get; set; }

    [Required]
    public required string MasterConnectionString { get; set; }

    [Required]
    public required string DbName { get; set; }
}
