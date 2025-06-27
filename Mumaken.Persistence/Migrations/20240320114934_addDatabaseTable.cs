using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class addDatabaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AspNetUsers_RentalCompanyId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarCategory_CarCategoryId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarModel_CarModelId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarType_CarTypeId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliverCompany_AspNetUsers_DeliverCompanyId",
                table: "OrderDeliverCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliverCompany_Orders_OrderId",
                table: "OrderDeliverCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliverCompany",
                table: "OrderDeliverCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarType",
                table: "CarType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarCategory",
                table: "CarCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car",
                table: "Car");

            migrationBuilder.RenameTable(
                name: "OrderDeliverCompany",
                newName: "OrderDeliverCompanies");

            migrationBuilder.RenameTable(
                name: "CarType",
                newName: "CarTypes");

            migrationBuilder.RenameTable(
                name: "CarModel",
                newName: "CarModels");

            migrationBuilder.RenameTable(
                name: "CarCategory",
                newName: "CarCategories");

            migrationBuilder.RenameTable(
                name: "Car",
                newName: "Cars");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliverCompany_OrderId",
                table: "OrderDeliverCompanies",
                newName: "IX_OrderDeliverCompanies_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliverCompany_DeliverCompanyId",
                table: "OrderDeliverCompanies",
                newName: "IX_OrderDeliverCompanies_DeliverCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_RentalCompanyId",
                table: "Cars",
                newName: "IX_Cars_RentalCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_CarTypeId",
                table: "Cars",
                newName: "IX_Cars_CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_CarModelId",
                table: "Cars",
                newName: "IX_Cars_CarModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_CarCategoryId",
                table: "Cars",
                newName: "IX_Cars_CarCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliverCompanies",
                table: "OrderDeliverCompanies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarCategories",
                table: "CarCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataForDeliverApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliverCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataForDeliverApps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataForDeliverApps_AspNetUsers_DeliverCompanyId",
                        column: x => x.DeliverCompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DataForDeliverApps_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderCars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalPriceDaily = table.Column<double>(type: "float", nullable: false),
                    InsuranceInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileInsurancInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarForm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarCategoryId = table.Column<int>(type: "int", nullable: false),
                    CarModelId = table.Column<int>(type: "int", nullable: false),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
                    RentalCompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCars_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OrderCycleIntros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCycleIntros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenewOrderdata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewOrderdata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RenewOrderdata_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Distract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distract_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DataForDeliverApps_DeliverCompanyId",
                table: "DataForDeliverApps",
                column: "DeliverCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DataForDeliverApps_UserId",
                table: "DataForDeliverApps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Distract_CityId",
                table: "Distract",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCars_OrderId",
                table: "OrderCars",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RenewOrderdata_OrderId",
                table: "RenewOrderdata",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_City_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_RentalCompanyId",
                table: "Cars",
                column: "RentalCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategories_CarCategoryId",
                table: "Cars",
                column: "CarCategoryId",
                principalTable: "CarCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliverCompanies_AspNetUsers_DeliverCompanyId",
                table: "OrderDeliverCompanies",
                column: "DeliverCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliverCompanies_Orders_OrderId",
                table: "OrderDeliverCompanies",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_City_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_RentalCompanyId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategories_CarCategoryId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliverCompanies_AspNetUsers_DeliverCompanyId",
                table: "OrderDeliverCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliverCompanies_Orders_OrderId",
                table: "OrderDeliverCompanies");

            migrationBuilder.DropTable(
                name: "DataForDeliverApps");

            migrationBuilder.DropTable(
                name: "Distract");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "OrderCars");

            migrationBuilder.DropTable(
                name: "OrderCycleIntros");

            migrationBuilder.DropTable(
                name: "RenewOrderdata");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliverCompanies",
                table: "OrderDeliverCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarCategories",
                table: "CarCategories");

            migrationBuilder.RenameTable(
                name: "OrderDeliverCompanies",
                newName: "OrderDeliverCompany");

            migrationBuilder.RenameTable(
                name: "CarTypes",
                newName: "CarType");

            migrationBuilder.RenameTable(
                name: "Cars",
                newName: "Car");

            migrationBuilder.RenameTable(
                name: "CarModels",
                newName: "CarModel");

            migrationBuilder.RenameTable(
                name: "CarCategories",
                newName: "CarCategory");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliverCompanies_OrderId",
                table: "OrderDeliverCompany",
                newName: "IX_OrderDeliverCompany_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliverCompanies_DeliverCompanyId",
                table: "OrderDeliverCompany",
                newName: "IX_OrderDeliverCompany_DeliverCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_RentalCompanyId",
                table: "Car",
                newName: "IX_Car_RentalCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarTypeId",
                table: "Car",
                newName: "IX_Car_CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarModelId",
                table: "Car",
                newName: "IX_Car_CarModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarCategoryId",
                table: "Car",
                newName: "IX_Car_CarCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliverCompany",
                table: "OrderDeliverCompany",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarType",
                table: "CarType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car",
                table: "Car",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarCategory",
                table: "CarCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AspNetUsers_RentalCompanyId",
                table: "Car",
                column: "RentalCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarCategory_CarCategoryId",
                table: "Car",
                column: "CarCategoryId",
                principalTable: "CarCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarModel_CarModelId",
                table: "Car",
                column: "CarModelId",
                principalTable: "CarModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarType_CarTypeId",
                table: "Car",
                column: "CarTypeId",
                principalTable: "CarType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliverCompany_AspNetUsers_DeliverCompanyId",
                table: "OrderDeliverCompany",
                column: "DeliverCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliverCompany_Orders_OrderId",
                table: "OrderDeliverCompany",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
