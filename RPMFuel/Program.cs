using RPMFuel;
using RPMFuel.Config;
using RPMFuel.HttpClients;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddOptions<WorkerConfigOptions>()
            .Bind(builder.Configuration.GetSection(WorkerConfigOptions.Name));
builder.Services.AddOptions<EIAClientConfigOptions>()
            .Bind(builder.Configuration.GetSection(EIAClientConfigOptions.Name));
builder.Services.AddTransient<PetrolService>();
builder.Services.AddHttpClient<EIAClient>();

var host = builder.Build();
host.Run();
