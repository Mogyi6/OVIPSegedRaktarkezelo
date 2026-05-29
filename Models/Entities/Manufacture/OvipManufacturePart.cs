using Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Manufacture
{
    public class OvipManufacturePart
    {
        public int Id { get; set; }

        public int ManufactureId { get; set; }

        public OvipManufacture Manufacture { get; set; } = null!;

        public int PartProductId { get; set; }

        public OvipProduct PartProduct { get; set; } = null!;

        public decimal Quantity { get; set; }
    }
}
