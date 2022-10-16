using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Wpf.Models
{
    internal class DeviceItem
    {
        public string DeviceId { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string Location { get; set; } = "";
        public bool DeviceState { get; set; } = false;
        public object Interval { get; internal set; }
    }
}
