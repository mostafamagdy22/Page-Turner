using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurner.Migrations
{
    /// <inheritdoc />
    public partial class setidasprimarykeytobooktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {




            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookISBN",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_BookISBN",
                table: "OrderDetails");


            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCategory",
                table: "BookCategory");

           

            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.AddColumn<int>(
                name: "BooksID",
                table: "BookCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);


         

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCategory",
                table: "BookCategory",
                columns: new[] { "BooksID", "CategoriesID" });

           

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookID",
                table: "Reviews",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_BookID",
                table: "OrderDetails",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_bookAuthors_Books_BookID",
                table: "bookAuthors",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategory_Books_BooksID",
                table: "BookCategory",
                column: "BooksID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Books_BookID",
                table: "OrderDetails",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookID",
                table: "Reviews",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookAuthors_Books_BookID",
                table: "bookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCategory_Books_BooksID",
                table: "BookCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Books_BookID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BookID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_BookID",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCategory",
                table: "BookCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookAuthors",
                table: "bookAuthors");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BooksID",
                table: "BookCategory");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "bookAuthors");

            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BooksISBN",
                table: "BookCategory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                table: "bookAuthors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "ISBN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCategory",
                table: "BookCategory",
                columns: new[] { "BooksISBN", "CategoriesID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookAuthors",
                table: "bookAuthors",
                columns: new[] { "BookISBN", "AuthorID" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookISBN",
                table: "Reviews",
                column: "BookISBN");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_BookISBN",
                table: "OrderDetails",
                column: "BookISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_bookAuthors_Books_BookISBN",
                table: "bookAuthors",
                column: "BookISBN",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategory_Books_BooksISBN",
                table: "BookCategory",
                column: "BooksISBN",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Books_BookISBN",
                table: "OrderDetails",
                column: "BookISBN",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookISBN",
                table: "Reviews",
                column: "BookISBN",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
