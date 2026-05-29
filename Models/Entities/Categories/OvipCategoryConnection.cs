using Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Categories
{
    public class OvipCategoryConnection
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public OvipProduct Product { get; set; } = null!;

        public int OvipCategoryId { get; set; }

        public OvipCategory Category { get; set; } = null!;
    }
}
