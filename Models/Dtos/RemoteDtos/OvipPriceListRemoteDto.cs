using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipPriceListRemoteDto
    {
        public int price_list_ovip_id { get; set; }
        public string? price_list_name { get; set; }
        public List<OvipPriceListPriceRemoteDto> price_list_prices { get; set; } = new();
    }
}
