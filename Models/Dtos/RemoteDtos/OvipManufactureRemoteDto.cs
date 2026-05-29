using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RemoteDtos
{
    public class OvipManufactureRemoteDto
    {
        public int ovip_product_id { get; set; }
        public int auto_complete { get; set; }
        public List<OvipManufacturePartRemoteDto> parts { get; set; } = new();
    }
}
