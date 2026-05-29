using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipCategoryRemoteDto
    {
        public int ovip_category_id { get; set; }
        public int? parent_category_id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? seo_title { get; set; }
        public string? seo_description { get; set; }
        public string? image { get; set; }
        public int order { get; set; }
    }
}
