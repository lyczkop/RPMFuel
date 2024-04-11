using RPMFuel;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddOptions<JobConfigOptions>()
            .Bind(builder.Configuration.GetSection(JobConfigOptions.Name));

var host = builder.Build();
host.Run();
