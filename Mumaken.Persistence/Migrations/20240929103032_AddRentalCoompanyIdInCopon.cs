using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddRentalCoompanyIdInCopon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RentalCompanyId",
                table: "Copon",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Copon_RentalCompanyId",
                table: "Copon",
                column: "RentalCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Copon_AspNetUsers_RentalCompanyId",
                table: "Copon",
                column: "RentalCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copon_AspNetUsers_RentalCompanyId",
                table: "Copon");

            migrationBuilder.DropIndex(
                name: "IX_Copon_RentalCompanyId",
                table: "Copon");

            migrationBuilder.DropColumn(
                name: "RentalCompanyId",
                table: "Copon");
        }
    }
}
