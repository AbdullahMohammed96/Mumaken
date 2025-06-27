using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddUserDataInOrderDeliverTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserDataInAppDelivery",
                table: "OrderDeliverCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WorkWithDeliveryCompany",
                table: "OrderDeliverCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDataInAppDelivery",
                table: "OrderDeliverCompanies");

            migrationBuilder.DropColumn(
                name: "WorkWithDeliveryCompany",
                table: "OrderDeliverCompanies");
        }
    }
}
