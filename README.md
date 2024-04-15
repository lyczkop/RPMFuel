# RPMFuel

It's a project to fetch petrol data from `https://api.eia.gov` and save in database

- worker is running every X seconds (default: 2), can be set in appsettings.json
- always fetching all data
- saves only data from last N days (default: 10)
  - if record already exists in database - ignore it, duplicates can be checked by the period field
- data are saved in format (Id, Period, Value, Units)
  - TODO Value and Units can be saved as Value Object "Price"

### Prerequisites

Complete appsettings.json

1. SqlServerConfigOptions.ConnectionStrings
   - MasterConnectionString needed to perform creating database if it's not exists
   - ConnectionString your target DB
2. SqlServerConfigOptions.DbName - your target DB (from your ConnectionString)
3. EIAClientConfigOptions.ApiKey - needed to fetch data

### TODO in future

Since this is an "interview task", I left something to cover in future due to lack of time ;)

1. Add Polly to fetching data
2. Add exceptions management
   - find out some "null reference exceptions" and handle in proper way
3. Serilog
4. AutoMapper
5. Maybe Docker?
6. Integration tests - with docker
7. ...
