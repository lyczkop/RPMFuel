using FluentMigrator;

namespace RPMFuel.Infrastructure.Database.Migrations;

[Migration(202404150700)]
public class Migration_202404150700_CreateTableFuelData : Migration
{
    public override void Down()
    {
        Delete.Table("FuelData");
    }

    public override void Up()
    {
        Create.Table("FuelData")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Period").AsDate().NotNullable()
            //.WithColumn("Value").AsDecimal().NotNullable()
            //.WithColumn("Units").AsString().NotNullable();
            .WithColumn("Price").AsString().NotNullable();
    }
}
