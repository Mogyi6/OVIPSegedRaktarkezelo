using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Pricing
{
    public class OvipPriceList
    {
        public int OvipPriceListId { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<OvipPriceListPrice> Prices { get; set; } = [];
    }
}
