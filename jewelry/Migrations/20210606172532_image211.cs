using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class image211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CarouselImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageNumber = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    CarImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarouselImage_Image_CarImageId",
                        column: x => x.CarImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarouselImage_CarImageId",
                table: "CarouselImage",
                column: "CarImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarouselImage");

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
    }
}
