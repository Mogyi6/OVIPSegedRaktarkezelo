using Models.Entities.Categories;
using Models.Entities.Manufacture;
using Models.Entities.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Products
{
    public class OvipProduct
    {
        // =========================
        // ALAP ADATOK
        // =========================

        public int OvipProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        public string? ManufactureSku { get; set; }

        public string? Barcode { get; set; }

        public string? Manufacturer { get; set; }

        public bool Deleted { get; set; }

        public bool WebshopVisible { get; set; }

        public int Orderable { get; set; }



        // =========================
        // LEÍRÁSOK
        // =========================

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }



        // =========================
        // MÉRETEK / SÚLY
        // =========================

        public decimal? NetWeight { get; set; }

        public decimal? GrossWeight { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }



        // =========================
        // EGYSÉGEK
        // =========================

        public string? Unit { get; set; }

        public string? AltUnit { get; set; }

        public decimal? AltUnitQuantity { get; set; }

        public decimal? ProductUnitQuantity { get; set; }



        // =========================
        // ÁRAK
        // =========================

        public decimal NetPrice { get; set; }

        public decimal GrossPrice { get; set; }

        public decimal? NetSalePrice { get; set; }

        public decimal? GrossSalePrice { get; set; }

        public decimal Tax { get; set; }

        public DateTime? SaleStart { get; set; }

        public DateTime? SaleEnd { get; set; }



        // =========================
        // BESZERZÉSI ÁRAK
        // =========================

        public decimal? AcquisitionCostHuf { get; set; }

        public decimal? AcquisitionCostForeign { get; set; }

        public string? AcquisitionCostCurrency { get; set; }

        public decimal? AcquisitionCostExchange { get; set; }



        // =========================
        // KATEGÓRIÁK
        // =========================

        public int OvipCategoryId { get; set; }

        public OvipCategory? MainCategory { get; set; }

        public List<OvipCategoryConnection> Categories { get; set; } = [];



        // =========================
        // VARIÁNSOK
        // =========================

        public int? ProductVariantId { get; set; }

        public OvipProduct? VariantProduct { get; set; }



        // =========================
        // PARAMÉTEREK
        // =========================

        public List<OvipProductParameter> Parameters { get; set; } = [];



        // =========================
        // KÉPEK
        // =========================

        public List<OvipProductImage> Images { get; set; } = [];



        // =========================
        // KÉSZLET
        // =========================

        public OvipStock? Stock { get; set; }



        // =========================
        // MENNYISÉGI KEDVEZMÉNYEK
        // =========================

        public List<OvipQuantityDiscount> QuantityDiscounts { get; set; } = [];



        // =========================
        // ÁRLISTÁK
        // =========================

        public List<OvipPriceListPrice> PricelistPrices { get; set; } = [];



        // =========================
        // RECEPTÚRA / GYÁRTÁS
        // =========================

        public OvipManufacture? Manufacture { get; set; }



        // =========================
        // AUDIT
        // =========================

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
