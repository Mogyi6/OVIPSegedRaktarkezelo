using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Categories
{
    public class OvipCategoryConnectionCreateDto
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public int OvipCategoryId { get; set; }
    }
}
