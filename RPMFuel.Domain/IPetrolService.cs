
namespace RPMFuel.Domain
{
    public interface IPetrolService
    {
        Task UpdatePrices(CancellationToken cancellationToken);
    }
}