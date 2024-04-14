namespace RPMFuel.Domain.Models;

public record FuelDto(DateOnly Period, decimal Value, string Units);
