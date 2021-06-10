using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class carousel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageNumber",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Image",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ImageNumber",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Image");
        }
    }
}
