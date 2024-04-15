using Dapper;
using RPMFuel.Domain.Interfaces;
using RPMFuel.Domain.Models;
using RPMFuel.Infrastructure.Database.Entities;

namespace RPMFuel.Infrastructure.Database;

public class FuelRepository : IFuelRepository
{
    private const string GetAllFuelData = "SELECT * FROM FuelData";
    private const string InsertFuelData = "INSERT INTO FuelData(Period, Value, Units) VALUES (@Period, @Value, @Units)";
    private readonly DapperContext _dapperContext;

    public FuelRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task AddManyAsync(IEnumerable<FuelDto> fuelDtos)
    {
        using var connection = _dapperContext.CreateConnection();
        var entitiesToAdd = fuelDtos.Select(dto => new FuelDataEntity(dto.Period, dto.Value, dto.Units))
            .ToList();

        await connection.ExecuteAsync(InsertFuelData, entitiesToAdd);
    }

    public async Task<ICollection<FuelDto>> GetAllAsync()
    {
        using var connection = _dapperContext.CreateConnection();

        var results = (await connection.QueryAsync<FuelDataEntity>(GetAllFuelData))
            .Select(entity => new FuelDto(
                entity.Period,
                entity.Value,
                entity.Units));

        return results.ToList();
    }
}
