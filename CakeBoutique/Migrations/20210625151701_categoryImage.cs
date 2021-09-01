using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CakeBoutique.Migrations
{
    public partial class categoryImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "imageId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_imageId",
                table: "Category",
                column: "imageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Image_imageId",
                table: "Category",
                column: "imageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Image_imageId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_imageId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "imageId",
                table: "Category");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Category",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
