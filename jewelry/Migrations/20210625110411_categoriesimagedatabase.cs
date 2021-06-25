using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jewelry.Migrations
{
    public partial class categoriesimagedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CarImageId",
                table: "CarouselImage");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "CarouselImage");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "CarouselImage");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Category",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarImageId",
                table: "CarouselImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "CarouselImage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "CarouselImage",
                type: "int",
                nullable: true);
        }
    }
}
