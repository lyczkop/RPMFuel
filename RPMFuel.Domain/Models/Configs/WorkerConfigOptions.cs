namespace RPMFuel.Domain.Models.Configs;

public class WorkerConfigOptions
{
    public const string Name = "WorkerConfigOptions";
    public int DelayInSeconds { get; set; } = 2;
    public int DaysBehind { get; set; } = 10;
}
