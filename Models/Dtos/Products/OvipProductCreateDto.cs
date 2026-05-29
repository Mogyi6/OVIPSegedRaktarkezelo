using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Products
{
    public class OvipProductCreateDto
    {
        public int OvipProductId { get; set; }
        // =========================
        // ALAP ADATOK
        // =========================
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? ManufactureSku { get; set; }
        public string? Barcode { get; set; }
        public string? Manufacturer { get; set; }

        public bool WebshopVisible { get; set; } = true;
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
        public decimal Tax { get; set; }

        public decimal? NetSalePrice { get; set; }
        public decimal? GrossSalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        // =========================
        // KATEGÓRIA
        // =========================
        public int OvipCategoryId { get; set; }
    }
}
