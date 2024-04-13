namespace RPMFuel.Infrastructure.Database.Entities;

public class FuelDataEntity
{
    /// <summary>
    /// Parameterless consntructor is need for fetching typed data from DB
    /// </summary>
    private FuelDataEntity() { }

    public FuelDataEntity(DateOnly period, decimal value, string units)
    {
        Period = period;
        Price = $"{value}{units}";
    }

    public int Id { get; init; }
    public DateOnly Period { get; init; }
    public string Price { get; init; }
}