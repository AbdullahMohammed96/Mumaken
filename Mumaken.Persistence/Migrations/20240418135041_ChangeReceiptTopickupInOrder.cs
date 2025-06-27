using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class ChangeReceiptTopickupInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationReceiptCar",
                table: "Orders",
                newName: "LocationpickupCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationpickupCar",
                table: "Orders",
                newName: "LocationReceiptCar");
        }
    }
}
