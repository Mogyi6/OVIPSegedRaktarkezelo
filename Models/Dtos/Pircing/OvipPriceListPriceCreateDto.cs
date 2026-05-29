using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Pircing
{
    public class OvipPriceListPriceCreateDto
    {
        public int Id { get; set; }

        public int OvipPriceListId { get; set; }

        public int OvipProductId { get; set; }

        public decimal NetPrice { get; set; }

        public decimal GrossPrice { get; set; }

        public decimal? NetSalePrice { get; set; }

        public decimal? GrossSalePrice { get; set; }

        public decimal Tax { get; set; }

        public DateTime? SaleStart { get; set; }

        public DateTime? SaleEnd { get; set; }
    }
}
