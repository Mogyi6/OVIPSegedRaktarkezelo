using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Pricing
{
    public class OvipQuantityDiscount
    {
        public int OvipQuantityId { get; set; }

        public int ProductId { get; set; }

        public int PriceListId { get; set; }

        public decimal DiscountFromQuantity { get; set; }

        public decimal DiscountUntilQuantity { get; set; }

        public string DiscountType { get; set; } = string.Empty;

        public decimal DiscountValue { get; set; }
    }
}
