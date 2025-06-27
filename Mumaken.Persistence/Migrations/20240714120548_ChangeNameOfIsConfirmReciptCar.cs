using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class ChangeNameOfIsConfirmReciptCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmReciprCar",
                table: "Orders",
                newName: "IsConfirmReciptCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmReciptCar",
                table: "Orders",
                newName: "IsConfirmReciprCar");
        }
    }
}
