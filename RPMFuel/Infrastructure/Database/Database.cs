using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RPMFuel.Infrastructure.Config;

namespace RPMFuel.Infrastructure.Database;
public static class Database
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scoped = host.Services.CreateScope();

        EnsureDatabase(scoped);

        var runner = scoped.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        runner.MigrateUp();

        return host;
    }

    private static void EnsureDatabase(IServiceScope scope)
    {
        var sqlConfig = scope.ServiceProvider
            .GetRequiredService<IOptions<SqlServerConfigOptions>>().Value;

        var parameters = new DynamicParameters();
        parameters.Add("name", sqlConfig.DbName);
        using var connection = new SqlConnection(sqlConfig.MasterConnectionString);
        var records = connection.Query("SELECT * FROM sys.databases WHERE name = @name",
             parameters);
        if (!records.Any())
        {
            connection.Execute($"CREATE DATABASE {sqlConfig.DbName}");
        }
    }
}
