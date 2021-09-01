using Microsoft.EntityFrameworkCore.Migrations;

namespace CakeBoutique.Migrations
{
    public partial class image21312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageNumber",
                table: "CarouselImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageNumber",
                table: "CarouselImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
