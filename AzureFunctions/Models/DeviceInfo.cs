using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    public class DeviceInfo
    {
        public string messageId { get; set; }
        public string deviceId { get; set; }
        public string temperature { get; set; }
        public string humidity { get; set; }
    }
}
