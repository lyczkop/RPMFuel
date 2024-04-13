using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RPMFuel.Infrastructure.Config;
using System.Data;

namespace RPMFuel.Infrastructure.Database;

public class DapperContext
{
    private readonly string _connectionString;
    private readonly SqlServerConfigOptions _sqlOptions;

    public DapperContext(IOptions<SqlServerConfigOptions> options)
    {
        _sqlOptions = options.Value;
        _connectionString = _sqlOptions.ConnectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
