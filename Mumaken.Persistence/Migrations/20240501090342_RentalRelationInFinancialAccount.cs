using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class RentalRelationInFinancialAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RentalCompanyId",
                table: "FinancialAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccounts_RentalCompanyId",
                table: "FinancialAccounts",
                column: "RentalCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_AspNetUsers_RentalCompanyId",
                table: "FinancialAccounts",
                column: "RentalCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_AspNetUsers_RentalCompanyId",
                table: "FinancialAccounts");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccounts_RentalCompanyId",
                table: "FinancialAccounts");

            migrationBuilder.DropColumn(
                name: "RentalCompanyId",
                table: "FinancialAccounts");
        }
    }
}
