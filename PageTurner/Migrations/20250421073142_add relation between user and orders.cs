using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurner.Migrations
{
    /// <inheritdoc />
    public partial class addrelationbetweenuserandorders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
	        name: "UserID",
	        table: "Orders",
	        type: "nvarchar(450)",
	        nullable: false,
	        defaultValue: "");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_UserID",
				table: "Orders",
				column: "UserID");

			migrationBuilder.AddForeignKey(
				name: "FK_Orders_AspNetUsers_UserID",
				table: "Orders",
				column: "UserID",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Orders");
        }
    }
}
