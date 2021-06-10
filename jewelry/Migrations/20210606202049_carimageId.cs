using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class carimageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarouselImage_Image_CarImageId",
                table: "CarouselImage");

            migrationBuilder.DropIndex(
                name: "IX_CarouselImage_CarImageId",
                table: "CarouselImage");

            migrationBuilder.AlterColumn<int>(
                name: "CarImageId",
                table: "CarouselImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CarImageId",
                table: "CarouselImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CarouselImage_CarImageId",
                table: "CarouselImage",
                column: "CarImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarouselImage_Image_CarImageId",
                table: "CarouselImage",
                column: "CarImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
