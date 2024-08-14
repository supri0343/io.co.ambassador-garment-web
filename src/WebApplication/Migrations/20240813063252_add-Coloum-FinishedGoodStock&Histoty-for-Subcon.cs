using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addColoumFinishedGoodStockHistotyforSubcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinishedFrom",
                table: "GarmentFinishedGoodStocks",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingOutDetailId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingOutItemId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LoadingId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LoadingItemId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutItemId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedFrom",
                table: "GarmentFinishedGoodStocks");

            migrationBuilder.DropColumn(
                name: "CuttingOutDetailId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "CuttingOutItemId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "LoadingId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "LoadingItemId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "SewingOutId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "SewingOutItemId",
                table: "GarmentFinishedGoodStockHistories");
        }
    }
}
