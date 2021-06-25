using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class imgDataBaseCarousel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "Image",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "CarouselImage",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "CarouselImage");
        }
    }
}
