using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Pircing
{
    public class OvipPriceListCreateDto
    {
        public int OvipPriceListId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
