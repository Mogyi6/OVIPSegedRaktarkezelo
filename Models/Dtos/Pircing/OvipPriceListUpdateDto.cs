using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Pircing
{
    public class OvipPriceListUpdateDto
    {
        public int OvipPriceListId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
