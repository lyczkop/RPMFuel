using FluentMigrator.Runner;
using RPMFuel;
using RPMFuel.Infrastructure.Config;
using RPMFuel.Infrastructure.Database;
using RPMFuel.Infrastructure.Database.Migrations;
using RPMFuel.Infrastructure.HttpClients;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOptions<WorkerConfigOptions>()
    .BindConfiguration(WorkerConfigOptions.Name);
builder.Services.AddOptions<EIAClientConfigOptions>()
    .BindConfiguration(EIAClientConfigOptions.Name)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<SqlServerConfigOptions>()
    .BindConfiguration(SqlServerConfigOptions.Name)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<EIAClient>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<PetrolService>();
builder.Services.AddTransient<FuelRepository>();

var sqlServerConfigOptions = builder.Configuration
    .GetRequiredSection(SqlServerConfigOptions.Name)
    .Get<SqlServerConfigOptions>()!;

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddSqlServer()
        .WithGlobalConnectionString(sqlServerConfigOptions.ConnectionString)
        .WithMigrationsIn(typeof(Migration_202404150700_CreateTableFuelData).Assembly);
    });

var host = builder.Build();

host.MigrateDatabase()
    .Run();
