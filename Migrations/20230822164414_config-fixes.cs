using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintToCash.Migrations
{
    public partial class configfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Configuration",
                keyColumn: "Id",
                keyValue: 1,
                column: "PrinterElectricityConsumptionKW",
                value: 0.36m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Configuration",
                keyColumn: "Id",
                keyValue: 1,
                column: "PrinterElectricityConsumptionKW",
                value: 360m);
        }
    }
}
