using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.Parameters
{
    public class OvipParameterUpdateDto
    {
        public int OvipParameterId { get; set; }

        public string ParameterName { get; set; } = string.Empty;
    }
}
