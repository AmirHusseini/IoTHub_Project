using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.MVVM.Models
{
    internal class AddDeviceResponse
    {
        public string Message { get; set; } 
        public string DeviceConnectionString { get; set; }
        public string IotHubName { get; set; }
        public Twin DeviceTwin { get; set; }
    }
}
