using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddOrderPricesInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AppPercentage",
                table: "Settings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VatPercetage",
                table: "Settings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AppPercetage",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AppPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VatPercetage",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VatPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "coponPrice",
                table: "Orders",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppPercentage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "VatPercetage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AppPercetage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "NetPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VatPercetage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "coponPrice",
                table: "Orders");
        }
    }
}
