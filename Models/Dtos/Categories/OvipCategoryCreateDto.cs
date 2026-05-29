using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Categories
{
    public class OvipCategoryCreateDto
    {
        public int OvipCategoryId { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }

        public string? Image { get; set; }

        public int Order { get; set; }
    }
}
