using Dapper;
using RPMFuel.Infrastructure.Database.Entities;

namespace RPMFuel.Infrastructure.Database;

public class FuelRepository
{
    private const string GetAllFuelData = "SELECT * FROM FuelData";
    private const string InsertFuelData = "INSERT INTO FuelData(Period, Price) VALUES (@Period, @Price)";
    private readonly DapperContext _dapperContext;

    public FuelRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task AddManyAsync(IEnumerable<FuelDataEntity> fuelData)
    {
        using var connection = _dapperContext.CreateConnection();

        await connection.ExecuteAsync(InsertFuelData, fuelData);
    }

    public async Task<ICollection<FuelDataEntity>> GetAllAsync()
    {
        using var connection = _dapperContext.CreateConnection();

        var results = await connection.QueryAsync<FuelDataEntity>(GetAllFuelData);

        return results.ToList();
    }
}
