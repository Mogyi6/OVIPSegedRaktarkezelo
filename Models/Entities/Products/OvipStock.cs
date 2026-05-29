using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Products
{
    public class OvipStock
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public OvipProduct Product { get; set; } = null!;

        public decimal Stock { get; set; }

        public decimal FreeStock { get; set; }

        public string? Unit { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
