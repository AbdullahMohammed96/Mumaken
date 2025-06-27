using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddRentalCompanyInReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RentalCompayId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RentalCompayId",
                table: "Reports",
                column: "RentalCompayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_RentalCompayId",
                table: "Reports",
                column: "RentalCompayId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_RentalCompayId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_RentalCompayId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RentalCompayId",
                table: "Reports");
        }
    }
}
