using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Manufacture
{
    public class OvipManufacturePartUpdateDto
    {
        public int Id { get; set; }
        public int ManufactureId { get; set; }
        public int PartProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
