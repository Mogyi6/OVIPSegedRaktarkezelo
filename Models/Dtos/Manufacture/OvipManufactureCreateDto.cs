using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Manufacture
{
    public class OvipManufactureCreateDto
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public bool AutoComplete { get; set; }
    }
}
