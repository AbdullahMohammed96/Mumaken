using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class UserDateInCompanyAppInOrderDeliver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderCars_OrderId",
                table: "OrderCars");

            migrationBuilder.DropColumn(
                name: "UserDateInCompanyApp",
                table: "OrderDeliverCompanies");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCars_OrderId",
                table: "OrderCars",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderCars_OrderId",
                table: "OrderCars");

            migrationBuilder.AddColumn<string>(
                name: "UserDateInCompanyApp",
                table: "OrderDeliverCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCars_OrderId",
                table: "OrderCars",
                column: "OrderId");
        }
    }
}
