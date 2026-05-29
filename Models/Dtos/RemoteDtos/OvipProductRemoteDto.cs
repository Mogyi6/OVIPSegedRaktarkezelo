using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipProductRemoteDto
    {
        public int ovip_product_id { get; set; }
        public string? name { get; set; }
        public string? sku { get; set; }
        public int? ovip_category_id { get; set; }
        public int? product_variant_id { get; set; }
        public string? bar_code { get; set; }
        public string? unit { get; set; }
        public string? alt_unit { get; set; }
        public decimal? alt_unit_quantity { get; set; }
        public decimal? product_unit_quantity { get; set; }
        public string? manufacturer { get; set; }
        public decimal? net_weight { get; set; }
        public decimal? gross_weight { get; set; }
        public decimal? width { get; set; }
        public decimal? height { get; set; }
        public decimal? length { get; set; }
        public string? manufacture_sku { get; set; }
        public string? short_description { get; set; }
        public string? long_description { get; set; }
        public int? orderable { get; set; }
        public string? seo_title { get; set; }
        public string? seo_description { get; set; }
        public decimal? tax { get; set; }
        public decimal? net_price { get; set; }
        public decimal? gross_price { get; set; }
        public decimal? net_sale_price { get; set; }
        public decimal? gross_sale_price { get; set; }
        public DateTime? sale_start { get; set; }
        public DateTime? sale_end { get; set; }
        public decimal? acquisition_cost_huf { get; set; }
        public decimal? acquisition_cost_foreign { get; set; }
        public string? acquisition_cost_currency { get; set; }
        public decimal? acquisition_cost_exchange { get; set; }
        public int? webshop_visible { get; set; }
        public int? deleted { get; set; }
    }
}
