using Microsoft.EntityFrameworkCore.Migrations;

namespace CakeBoutique.Migrations
{
    public partial class productcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "size",
                table: "ProductCart");

            migrationBuilder.AddColumn<string>(
                name: "CustumName",
                table: "ProductCart",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ProductCart",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustumName",
                table: "ProductCart");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ProductCart");

            migrationBuilder.AddColumn<double>(
                name: "size",
                table: "ProductCart",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
