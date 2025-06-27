using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class RemoveCarFormAndInsuranc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarForm",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FileInsurancInformation",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "InsuranceInformation",
                table: "Cars");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarForm",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileInsurancInformation",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceInformation",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

          
        }
    }
}
