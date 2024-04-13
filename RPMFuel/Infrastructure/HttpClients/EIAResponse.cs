namespace RPMFuel.Infrastructure.HttpClients;

public record EIAResponse(EIAResponseMessage Response);
public record EIAResponseMessage(int Total, IEnumerable<FuelDto> Data);
