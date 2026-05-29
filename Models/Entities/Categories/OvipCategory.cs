using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Categories
{
    public class OvipCategory
    {
        public int OvipCategoryId { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }

        public string? Image { get; set; }

        public int Order { get; set; }



        // NAVIGATION

        public OvipCategory? ParentCategory { get; set; }

        public List<OvipCategory> Children { get; set; } = [];

        public List<OvipCategoryConnection> Products { get; set; } = [];
    }
}
