using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.MVVM.Models
{
    public class DeviceItem
    {
        public string DeviceId { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string Location { get; set; } = "";
        public string Owner { get; set; } = "";
        public string DeviceState { get; set; } = "";

        public string IconActive { get; set; } = "";
        public string IconInActive { get; set; } = "";
        public string StateActive { get; set; } = "";
        public string StateInActive { get; set; } = "";
    }
}
