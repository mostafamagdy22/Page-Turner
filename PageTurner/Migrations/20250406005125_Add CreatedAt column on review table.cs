using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurner.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtcolumnonreviewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCategory_Categories_CategoriesID",
                table: "BookCategory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategory_Categories_CategoriesID",
                table: "BookCategory",
                column: "CategoriesID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
