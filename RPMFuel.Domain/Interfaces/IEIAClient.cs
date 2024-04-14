using RPMFuel.Domain.Models;

namespace RPMFuel.Domain.Interfaces
{
    public interface IEIAClient
    {
        Task<ICollection<FuelDto>> GetPetrolData();
    }
}