using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddRelationWithConnectAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectUser_AspNetUsers_ApplicationDbUserId",
                table: "ConnectUser");

            migrationBuilder.DropIndex(
                name: "IX_ConnectUser_ApplicationDbUserId",
                table: "ConnectUser");

            migrationBuilder.DropColumn(
                name: "ApplicationDbUserId",
                table: "ConnectUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ConnectUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectUser_UserId",
                table: "ConnectUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectUser_AspNetUsers_UserId",
                table: "ConnectUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectUser_AspNetUsers_UserId",
                table: "ConnectUser");

            migrationBuilder.DropIndex(
                name: "IX_ConnectUser_UserId",
                table: "ConnectUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ConnectUser",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationDbUserId",
                table: "ConnectUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectUser_ApplicationDbUserId",
                table: "ConnectUser",
                column: "ApplicationDbUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectUser_AspNetUsers_ApplicationDbUserId",
                table: "ConnectUser",
                column: "ApplicationDbUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
