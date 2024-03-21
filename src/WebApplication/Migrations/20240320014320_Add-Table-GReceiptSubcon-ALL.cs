using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddTableGReceiptSubconALL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutInNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingType = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingInDate = table.Column<DateTimeOffset>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOuts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingOutType = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    EPOId = table.Column<long>(nullable: false),
                    EPOItemId = table.Column<long>(nullable: false),
                    POSerialNumber = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconExpenditureGoodReturns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ReturNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    ReturType = table.Column<string>(maxLength: 25, nullable: true),
                    PackingOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    DONo = table.Column<string>(maxLength: 50, nullable: true),
                    BCNo = table.Column<string>(maxLength: 50, nullable: true),
                    BCType = table.Column<string>(maxLength: 50, nullable: true),
                    URNNo = table.Column<string>(maxLength: 50, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 100, nullable: true),
                    ReturDate = table.Column<DateTimeOffset>(nullable: false),
                    Invoice = table.Column<string>(maxLength: 50, nullable: true),
                    ContractNo = table.Column<string>(nullable: true),
                    ReturDesc = table.Column<string>(maxLength: 500, nullable: true),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconExpenditureGoodReturns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishedGoodStocks",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishedGoodStockNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishedGoodStocks", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    FinishingInType = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    FinishingInDate = table.Column<DateTimeOffset>(nullable: false),
                    DOId = table.Column<long>(nullable: false),
                    DONo = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(nullable: true),
                    SubconType = table.Column<string>(maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOuts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    FinishingTo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitToId = table.Column<int>(nullable: false),
                    UnitToCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitToName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    FinishingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    IsDifferentSize = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    LoadingNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingOutId = table.Column<Guid>(nullable: false),
                    CuttingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityName = table.Column<string>(maxLength: 500, nullable: true),
                    ComodityCode = table.Column<string>(maxLength: 100, nullable: true),
                    LoadingDate = table.Column<DateTimeOffset>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingOuts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    LoadingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    LoadingInId = table.Column<Guid>(nullable: false),
                    LoadingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityName = table.Column<string>(maxLength: 500, nullable: true),
                    ComodityCode = table.Column<string>(maxLength: 100, nullable: true),
                    LoadingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    PackingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingFrom = table.Column<string>(nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 2000, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingInDate = table.Column<DateTimeOffset>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingOuts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    PackingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingOutType = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    Invoice = table.Column<string>(maxLength: 50, nullable: true),
                    PackingListId = table.Column<int>(nullable: false),
                    ContractNo = table.Column<string>(nullable: true),
                    Carton = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsReceived = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPreparings",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 500, nullable: true),
                    ProcessDate = table.Column<DateTimeOffset>(nullable: true),
                    RONo = table.Column<string>(maxLength: 100, nullable: true),
                    Article = table.Column<string>(maxLength: 500, nullable: true),
                    IsCuttingIn = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPreparings", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    SewingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    LoadingOutId = table.Column<Guid>(nullable: false),
                    LoadingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingInDate = table.Column<DateTimeOffset>(nullable: false),
                    UId = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOuts",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingTo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitToId = table.Column<int>(nullable: false),
                    UnitToCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitToName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    IsDifferentSize = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutInId = table.Column<Guid>(nullable: false),
                    PreparingId = table.Column<Guid>(nullable: false),
                    SewingOutId = table.Column<Guid>(nullable: false),
                    SewingOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingInItems_GarmentSubconCuttingIns_CutInId",
                        column: x => x.CutInId,
                        principalTable: "GarmentSubconCuttingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOutItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutOutId = table.Column<Guid>(nullable: false),
                    CuttingInId = table.Column<Guid>(nullable: false),
                    CuttingInDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    TotalCuttingOut = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true),
                    RealQtyOut = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingOutItems_GarmentSubconCuttingOuts_CutOutId",
                        column: x => x.CutOutId,
                        principalTable: "GarmentSubconCuttingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconExpenditureGoodReturnItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ReturId = table.Column<Guid>(nullable: false),
                    ExpenditureGoodId = table.Column<Guid>(nullable: false),
                    ExpenditureGoodItemId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconExpenditureGoodReturnItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconExpenditureGoodReturnItems_GarmentSubconExpenditureGoodReturns_ReturId",
                        column: x => x.ReturId,
                        principalTable: "GarmentSubconExpenditureGoodReturns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishingInId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    SubconCuttingId = table.Column<Guid>(nullable: false),
                    DODetailId = table.Column<long>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconFinishingInItems_GarmentSubconFinishingIns_FinishingInId",
                        column: x => x.FinishingInId,
                        principalTable: "GarmentSubconFinishingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOutItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishingOutId = table.Column<Guid>(nullable: false),
                    FinishingInId = table.Column<Guid>(nullable: false),
                    FinishingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    RealQtyOut = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Color = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconFinishingOutItems_GarmentSubconFinishingOuts_FinishingOutId",
                        column: x => x.FinishingOutId,
                        principalTable: "GarmentSubconFinishingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    LoadingId = table.Column<Guid>(nullable: false),
                    CuttingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 500, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconLoadingInItems_GarmentSubconLoadingIns_LoadingId",
                        column: x => x.LoadingId,
                        principalTable: "GarmentSubconLoadingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingOutItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    LoadingOutId = table.Column<Guid>(nullable: false),
                    LoadingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 500, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RealQtyOut = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconLoadingOutItems_GarmentSubconLoadingOuts_LoadingOutId",
                        column: x => x.LoadingOutId,
                        principalTable: "GarmentSubconLoadingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    PackingInId = table.Column<Guid>(nullable: false),
                    CuttingOutItemId = table.Column<Guid>(nullable: false),
                    CuttingOutDetailId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    FinishingOutItemId = table.Column<Guid>(nullable: false),
                    FinishingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Color = table.Column<string>(nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPackingInItems_GarmentSubconPackingIns_PackingInId",
                        column: x => x.PackingInId,
                        principalTable: "GarmentSubconPackingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingOutItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    PackingOutId = table.Column<Guid>(nullable: false),
                    PackingInItemId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReturQuantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPackingOutItems_GarmentSubconPackingOuts_PackingOutId",
                        column: x => x.PackingOutId,
                        principalTable: "GarmentSubconPackingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPreparingItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    UENItemId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    FabricType = table.Column<string>(maxLength: 100, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    GarmentSubconPreparingId = table.Column<Guid>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    ROSource = table.Column<string>(maxLength: 100, nullable: true),
                    BeacukaiNo = table.Column<string>(maxLength: 20, nullable: true),
                    BeacukaiDate = table.Column<DateTimeOffset>(nullable: false),
                    BeacukaiType = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPreparingItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPreparingItems_GarmentSubconPreparings_GarmentSubconPreparingId",
                        column: x => x.GarmentSubconPreparingId,
                        principalTable: "GarmentSubconPreparings",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingInId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    LoadingOutItemId = table.Column<Guid>(nullable: false),
                    FinishingOutItemId = table.Column<Guid>(nullable: false),
                    FinishingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconSewingInItems_GarmentSubconSewingIns_SewingInId",
                        column: x => x.SewingInId,
                        principalTable: "GarmentSubconSewingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOutItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingOutId = table.Column<Guid>(nullable: false),
                    SewingInId = table.Column<Guid>(nullable: false),
                    SewingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    RealQtyOut = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Color = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconSewingOutItems_GarmentSubconSewingOuts_SewingOutId",
                        column: x => x.SewingOutId,
                        principalTable: "GarmentSubconSewingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingInDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutInItemId = table.Column<Guid>(nullable: false),
                    PreparingItemId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    FabricType = table.Column<string>(maxLength: 25, nullable: true),
                    PreparingQuantity = table.Column<double>(nullable: false),
                    PreparingUomId = table.Column<int>(nullable: false),
                    PreparingUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    CuttingInQuantity = table.Column<int>(nullable: false),
                    CuttingInUomId = table.Column<int>(nullable: false),
                    CuttingInUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingInDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingInDetails_GarmentSubconCuttingInItems_CutInItemId",
                        column: x => x.CutInItemId,
                        principalTable: "GarmentSubconCuttingInItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOutDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CutOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingOutQuantity = table.Column<double>(nullable: false),
                    CuttingOutUomId = table.Column<int>(nullable: false),
                    CuttingOutUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RealQtyOut = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingOutDetails_GarmentSubconCuttingOutItems_CutOutItemId",
                        column: x => x.CutOutItemId,
                        principalTable: "GarmentSubconCuttingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOutDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishingOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    RealQtyOut = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconFinishingOutDetails_GarmentSubconFinishingOutItems_FinishingOutItemId",
                        column: x => x.FinishingOutItemId,
                        principalTable: "GarmentSubconFinishingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOutDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    RealQtyOut = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconSewingOutDetails_GarmentSubconSewingOutItems_SewingOutItemId",
                        column: x => x.SewingOutItemId,
                        principalTable: "GarmentSubconSewingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingInDetails_CutInItemId",
                table: "GarmentSubconCuttingInDetails",
                column: "CutInItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingInItems_CutInId",
                table: "GarmentSubconCuttingInItems",
                column: "CutInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingIns_CutInNo",
                table: "GarmentSubconCuttingIns",
                column: "CutInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOutDetails_CutOutItemId",
                table: "GarmentSubconCuttingOutDetails",
                column: "CutOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOutItems_CutOutId",
                table: "GarmentSubconCuttingOutItems",
                column: "CutOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOuts_CutOutNo",
                table: "GarmentSubconCuttingOuts",
                column: "CutOutNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconExpenditureGoodReturnItems_ReturId",
                table: "GarmentSubconExpenditureGoodReturnItems",
                column: "ReturId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconExpenditureGoodReturns_ReturNo",
                table: "GarmentSubconExpenditureGoodReturns",
                column: "ReturNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishedGoodStocks_FinishedGoodStockNo",
                table: "GarmentSubconFinishedGoodStocks",
                column: "FinishedGoodStockNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingInItems_FinishingInId",
                table: "GarmentSubconFinishingInItems",
                column: "FinishingInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingIns_FinishingInNo",
                table: "GarmentSubconFinishingIns",
                column: "FinishingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOutDetails_FinishingOutItemId",
                table: "GarmentSubconFinishingOutDetails",
                column: "FinishingOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOutItems_FinishingOutId",
                table: "GarmentSubconFinishingOutItems",
                column: "FinishingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOuts_FinishingOutNo",
                table: "GarmentSubconFinishingOuts",
                column: "FinishingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingInItems_LoadingId",
                table: "GarmentSubconLoadingInItems",
                column: "LoadingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingIns_LoadingNo",
                table: "GarmentSubconLoadingIns",
                column: "LoadingNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingOutItems_LoadingOutId",
                table: "GarmentSubconLoadingOutItems",
                column: "LoadingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingOuts_LoadingOutNo",
                table: "GarmentSubconLoadingOuts",
                column: "LoadingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingInItems_PackingInId",
                table: "GarmentSubconPackingInItems",
                column: "PackingInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingIns_PackingInNo",
                table: "GarmentSubconPackingIns",
                column: "PackingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingOutItems_PackingOutId",
                table: "GarmentSubconPackingOutItems",
                column: "PackingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingOuts_PackingOutNo",
                table: "GarmentSubconPackingOuts",
                column: "PackingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPreparingItems_GarmentSubconPreparingId",
                table: "GarmentSubconPreparingItems",
                column: "GarmentSubconPreparingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingInItems_SewingInId",
                table: "GarmentSubconSewingInItems",
                column: "SewingInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingIns_SewingInNo",
                table: "GarmentSubconSewingIns",
                column: "SewingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOutDetails_SewingOutItemId",
                table: "GarmentSubconSewingOutDetails",
                column: "SewingOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOutItems_SewingOutId",
                table: "GarmentSubconSewingOutItems",
                column: "SewingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOuts_SewingOutNo",
                table: "GarmentSubconSewingOuts",
                column: "SewingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingInDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconExpenditureGoodReturnItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishedGoodStocks");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPreparingItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconExpenditureGoodReturns");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingIns");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingIns");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingOuts");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingIns");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingOuts");

            migrationBuilder.DropTable(
                name: "GarmentSubconPreparings");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingIns");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingIns");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOuts");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOuts");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOuts");
        }
    }
}
