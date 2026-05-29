using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Products
{
    public class OvipProductImageCreateDto
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public string Image { get; set; } = string.Empty;

        public string? ThumbnailImage { get; set; }

        public bool MainImage { get; set; }

        public int SortOrder { get; set; }
    }
}
