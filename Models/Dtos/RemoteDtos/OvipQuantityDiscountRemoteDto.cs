using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipQuantityDiscountRemoteDto
    {
        public int ovip_quantity_id { get; set; }
        public int product_id { get; set; }
        public int price_list_id { get; set; }
        public decimal discount_from_quantity { get; set; }
        public decimal discount_until_quantity { get; set; }
        public string? discount_type { get; set; }
        public decimal discount_value { get; set; }
    }
}
