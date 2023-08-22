using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintToCash.Migrations
{
    public partial class configupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrinterElectricityConsumptionKW",
                table: "Configuration",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 360m);

            migrationBuilder.AddColumn<int>(
                name: "TaxPercentage",
                table: "Configuration",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Configuration",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PrinterElectricityConsumptionKW", "TaxPercentage" },
                values: new object[] { 360m, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrinterElectricityConsumptionKW",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                table: "Configuration");
        }
    }
}
