using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SOAPClient
{
    public class OvipOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string WebshopId { get; set; } = string.Empty;
        public string AuthCode { get; set; } = string.Empty;
        public string CallerIp { get; set; } = string.Empty;
    }
}
