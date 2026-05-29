using Models.Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Products
{
    public class OvipProductParameter
    {
        public int Id { get; set; }

        public int OvipProductId { get; set; }

        public OvipProduct Product { get; set; } = null!;

        public int OvipParameterId { get; set; }

        public OvipParameter Parameter { get; set; } = null!;

        public int OvipParameterValueId { get; set; }

        public string ParameterValue { get; set; } = string.Empty;
    }
}
