using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class RemoveApplicationDbUserFromHistoryNotify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryNotify_AspNetUsers_ApplicationDbUserId",
                table: "HistoryNotify");

            migrationBuilder.DropIndex(
                name: "IX_HistoryNotify_ApplicationDbUserId",
                table: "HistoryNotify");

            migrationBuilder.DropColumn(
                name: "ApplicationDbUserId",
                table: "HistoryNotify");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationDbUserId",
                table: "HistoryNotify",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryNotify_ApplicationDbUserId",
                table: "HistoryNotify",
                column: "ApplicationDbUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryNotify_AspNetUsers_ApplicationDbUserId",
                table: "HistoryNotify",
                column: "ApplicationDbUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
