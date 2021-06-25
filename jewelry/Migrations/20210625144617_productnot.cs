using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class productnot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateSum",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Rates",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "RateSum",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Rates",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
