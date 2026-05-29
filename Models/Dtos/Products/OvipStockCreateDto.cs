using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Products
{
    public class OvipStockCreateDto
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public decimal Stock { get; set; }

        public decimal FreeStock { get; set; }

        public string? Unit { get; set; }
    }
}
