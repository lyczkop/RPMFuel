using RPMFuel.Domain.Interfaces;

namespace RPMFuel.Infrastructure.Time
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
