using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddRentalDailyPeriodAndPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalConfirmationDelayPeriod",
                table: "RenewOrderdata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RentalConfirmationDelayPrice",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RentalConfirmationDelayPeriod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RentalConfirmationDelayPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalConfirmationDelayPeriod",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "RentalConfirmationDelayPrice",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "RentalConfirmationDelayPeriod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RentalConfirmationDelayPrice",
                table: "Orders");
        }
    }
}
