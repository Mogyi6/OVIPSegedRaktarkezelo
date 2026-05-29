using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipPriceListPriceRemoteDto
    {
        public int ovip_product_id { get; set; }
        public decimal net_price { get; set; }
        public decimal gross_price { get; set; }
        public decimal? net_sale_price { get; set; }
        public decimal? gross_sale_price { get; set; }
        public DateTime? sale_start { get; set; }
        public DateTime? sale_end { get; set; }
        public decimal tax { get; set; }
    }
}
