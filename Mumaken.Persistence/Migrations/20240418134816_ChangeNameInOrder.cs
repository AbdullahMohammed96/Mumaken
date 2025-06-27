using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class ChangeNameInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeReceiptCar",
                table: "Orders",
                newName: "TimePickupCar");

            migrationBuilder.RenameColumn(
                name: "DateReceiptCar",
                table: "Orders",
                newName: "DatePickupCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimePickupCar",
                table: "Orders",
                newName: "TimeReceiptCar");

            migrationBuilder.RenameColumn(
                name: "DatePickupCar",
                table: "Orders",
                newName: "DateReceiptCar");
        }
    }
}
