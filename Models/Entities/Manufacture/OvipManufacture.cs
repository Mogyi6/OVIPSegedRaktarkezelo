using Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Manufacture
{
    public class OvipManufacture
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public OvipProduct Product { get; set; } = null!;

        public bool AutoComplete { get; set; }

        public List<OvipManufacturePart> Parts { get; set; } = [];
    }
}
