using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Products
{
    public class OvipProductParameterCreateDto
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public int OvipParameterId { get; set; }

        public int OvipParameterValueId { get; set; }

        public string ParameterValue { get; set; } = string.Empty;
    }
}
