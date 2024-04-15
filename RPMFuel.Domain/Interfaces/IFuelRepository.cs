using RPMFuel.Domain.Models;

namespace RPMFuel.Domain.Interfaces
{
    public interface IFuelRepository
    {
        Task AddManyAsync(IEnumerable<FuelDto> fuelData);
        Task<ICollection<FuelDto>> GetAllAsync();
    }
}