using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_buyerBrand_SSubconCuttings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerBrandCode",
                table: "GarmentServiceSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerBrandId",
                table: "GarmentServiceSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerBrandName",
                table: "GarmentServiceSubconCuttings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerBrandCode",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "BuyerBrandId",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "BuyerBrandName",
                table: "GarmentServiceSubconCuttings");
        }
    }
}
