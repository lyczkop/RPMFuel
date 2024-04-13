namespace RPMFuel.Infrastructure.HttpClients;

public record FuelDto(DateOnly Period, decimal Value, string Units);
