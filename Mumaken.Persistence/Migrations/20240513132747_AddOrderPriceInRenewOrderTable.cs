using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddOrderPriceInRenewOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AppPercetage",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AppPrice",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CoponCode",
                table: "RenewOrderdata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountValueForDailyRental",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "RenewOrderdata",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NetPrice",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VatPercetage",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VatPrice",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "coponPrice",
                table: "RenewOrderdata",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "subTotal",
                table: "RenewOrderdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppPercetage",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "AppPrice",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "CoponCode",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "DiscountValueForDailyRental",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "NetPrice",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "VatPercetage",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "coponPrice",
                table: "RenewOrderdata");

            migrationBuilder.DropColumn(
                name: "subTotal",
                table: "RenewOrderdata");
        }
    }
}
