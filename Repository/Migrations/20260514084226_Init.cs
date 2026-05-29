using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    OvipCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.OvipCategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "OvipCategoryId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    OvipParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParameterName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.OvipParameterId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    OvipPriceListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.OvipPriceListId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    OvipProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sku = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManufactureSku = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Barcode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Manufacturer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WebshopVisible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Orderable = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LongDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoTitle = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoDescription = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NetWeight = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    GrossWeight = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Unit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltUnit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltUnitQuantity = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ProductUnitQuantity = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NetPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    GrossPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NetSalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    GrossSalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SaleStart = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SaleEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AcquisitionCostHuf = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AcquisitionCostForeign = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AcquisitionCostCurrency = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcquisitionCostExchange = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    OvipCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductVariantId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.OvipProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_OvipCategoryId",
                        column: x => x.OvipCategoryId,
                        principalTable: "Categories",
                        principalColumn: "OvipCategoryId");
                    table.ForeignKey(
                        name: "FK_Products_Products_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    OvipCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryConnections_Categories_OvipCategoryId",
                        column: x => x.OvipCategoryId,
                        principalTable: "Categories",
                        principalColumn: "OvipCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryConnections_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Manufactures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    AutoComplete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufactures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufactures_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceListPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipPriceListId = table.Column<int>(type: "int", nullable: false),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    GrossPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NetSalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    GrossSalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SaleStart = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SaleEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceListPrices_PriceLists_OvipPriceListId",
                        column: x => x.OvipPriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "OvipPriceListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceListPrices_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThumbnailImage = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainImage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    OvipParameterId = table.Column<int>(type: "int", nullable: false),
                    OvipParameterValueId = table.Column<int>(type: "int", nullable: false),
                    ParameterValue = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductParameters_Parameters_OvipParameterId",
                        column: x => x.OvipParameterId,
                        principalTable: "Parameters",
                        principalColumn: "OvipParameterId");
                    table.ForeignKey(
                        name: "FK_ProductParameters_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuantityDiscounts",
                columns: table => new
                {
                    OvipQuantityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: false),
                    DiscountFromQuantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiscountUntilQuantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiscountType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountValue = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    OvipProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityDiscounts", x => x.OvipQuantityId);
                    table.ForeignKey(
                        name: "FK_QuantityDiscounts_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OvipProductId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FreeStock = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Unit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_OvipProductId",
                        column: x => x.OvipProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ManufactureParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ManufactureId = table.Column<int>(type: "int", nullable: false),
                    PartProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufactureParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManufactureParts_Manufactures_ManufactureId",
                        column: x => x.ManufactureId,
                        principalTable: "Manufactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManufactureParts_Products_PartProductId",
                        column: x => x.PartProductId,
                        principalTable: "Products",
                        principalColumn: "OvipProductId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryConnections_OvipCategoryId",
                table: "CategoryConnections",
                column: "OvipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryConnections_OvipProductId_OvipCategoryId",
                table: "CategoryConnections",
                columns: new[] { "OvipProductId", "OvipCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufactureParts_ManufactureId",
                table: "ManufactureParts",
                column: "ManufactureId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufactureParts_PartProductId",
                table: "ManufactureParts",
                column: "PartProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufactures_OvipProductId",
                table: "Manufactures",
                column: "OvipProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceListPrices_OvipPriceListId",
                table: "PriceListPrices",
                column: "OvipPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListPrices_OvipProductId",
                table: "PriceListPrices",
                column: "OvipProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_OvipProductId",
                table: "ProductImages",
                column: "OvipProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductParameters_OvipParameterId",
                table: "ProductParameters",
                column: "OvipParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductParameters_OvipProductId",
                table: "ProductParameters",
                column: "OvipProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OvipCategoryId",
                table: "Products",
                column: "OvipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductVariantId",
                table: "Products",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityDiscounts_OvipProductId",
                table: "QuantityDiscounts",
                column: "OvipProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_OvipProductId",
                table: "Stocks",
                column: "OvipProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryConnections");

            migrationBuilder.DropTable(
                name: "ManufactureParts");

            migrationBuilder.DropTable(
                name: "PriceListPrices");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductParameters");

            migrationBuilder.DropTable(
                name: "QuantityDiscounts");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Manufactures");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
