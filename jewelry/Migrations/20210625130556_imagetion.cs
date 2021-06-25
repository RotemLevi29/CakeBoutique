using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class imagetion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagePath",
                table: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagePath",
                table: "Image",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
