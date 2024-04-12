using RPMFuel.Models;

namespace RPMFuel.HttpClients;

public record EIAResponse(EIAResponseMessage Response);
public record EIAResponseMessage(int Total, IEnumerable<FuelDto> Data);
