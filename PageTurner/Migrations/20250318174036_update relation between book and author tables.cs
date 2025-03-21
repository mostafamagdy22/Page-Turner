using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurner.Migrations
{
    /// <inheritdoc />
    public partial class updaterelationbetweenbookandauthortables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookAuthors_Authors_AuthorID",
                table: "bookAuthors");

            migrationBuilder.AddForeignKey(
                name: "FK_bookAuthors_Authors_AuthorID",
                table: "bookAuthors",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookAuthors_Authors_AuthorID",
                table: "bookAuthors");

            migrationBuilder.AddForeignKey(
                name: "FK_bookAuthors_Authors_AuthorID",
                table: "bookAuthors",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
