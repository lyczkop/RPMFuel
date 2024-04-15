namespace RPMFuel.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}