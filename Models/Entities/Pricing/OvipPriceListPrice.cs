using Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Pricing
{
    public class OvipPriceListPrice
    {
        public int Id { get; set; }

        public int OvipPriceListId { get; set; }

        public OvipPriceList PriceList { get; set; } = null!;

        public int OvipProductId { get; set; }

        public OvipProduct Product { get; set; } = null!;

        public decimal NetPrice { get; set; }

        public decimal GrossPrice { get; set; }

        public decimal? NetSalePrice { get; set; }

        public decimal? GrossSalePrice { get; set; }

        public decimal Tax { get; set; }

        public DateTime? SaleStart { get; set; }

        public DateTime? SaleEnd { get; set; }
    }
}
