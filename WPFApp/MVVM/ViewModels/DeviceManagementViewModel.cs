using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebApi.Services;
using WPFApp.MVVM.Cores;
using WPFApp.MVVM.Models;

namespace WPFApp.MVVM.ViewModels
{
    internal class DeviceManagementViewModel : BaseViewModel
    {
        private DispatcherTimer timer;
        private ObservableCollection<DeviceItem> _deviceItems;
        private readonly string baseurl = "https://iotproject-webapi.azurewebsites.net/api/devices/";
        public DeviceManagementViewModel()
        {
            _deviceItems = new ObservableCollection<DeviceItem>();
            PopulateDeviceItemsAsync().ConfigureAwait(false);
            SetInterval(TimeSpan.FromSeconds(3));
        }


        public string Title { get; set; } = "Device Management";
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public IEnumerable<DeviceItem> DeviceItems => _deviceItems;
        public List<Twin> Twins = new List<Twin>();


        private void SetInterval(TimeSpan interval)
        {
            timer = new DispatcherTimer()
            {
                Interval = interval
            };

            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private async void timer_tick(object sender, EventArgs e)
        {

            await PopulateDeviceItemsAsync();
            await UpdateDeviceItemsAsync();
        }


        private async Task UpdateDeviceItemsAsync()
        {
            using var client = new HttpClient();
            foreach (var item in _deviceItems.ToList())
            {
                var device = await client.GetAsync(baseurl + item.DeviceId);
                if (device == null)
                    _deviceItems.Remove(item);
            }
        }

        private async Task PopulateDeviceItemsAsync()
        {
            using var client = new HttpClient();
            var result = client.GetStringAsync(baseurl).Result;
            var s = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(result);
            foreach (var item in s)
            {
                
                //var t = await client.GetAsync(baseurl + "twin/" + item.deviceId);
                //var res3 = t.Content.ReadAsStringAsync().Result;
                //Twin twin = new Twin();
                //twin.DeviceId = res3.deviceId;
                //Twins.Add(twin);
            }
            var result1 = await client.GetAsync(baseurl);
            var res = result1.Content.ReadAsStringAsync().Result;
            //var json = JsonConvert.DeserializeObject<IEnumerable<Twin>>(res);

            //if (result != null)
            //{
            //    foreach (Twin twin in json.ToList())
            //    {
            //        var device = _deviceItems.FirstOrDefault(x => x.DeviceId == twin.DeviceId);

            //        if (device == null)
            //        {
            //            device = new DeviceItem
            //            {
            //                DeviceId = twin.DeviceId,
            //            };

            //            try { device.DeviceName = twin.Properties.Reported["deviceName"]; }
            //            catch { device.DeviceName = device.DeviceId; }
            //            try { device.DeviceType = twin.Properties.Reported["deviceType"]; }
            //            catch { }
            //            try { Temperature = twin.Properties.Desired["temperature"]; }
            //            catch { }
            //            try { Humidity = twin.Properties.Desired["humidity"]; }
            //            catch { }
            //            switch (device.DeviceType.ToLower())
            //            {
            //                case "fan":
            //                    device.IconActive = "\uf863";
            //                    device.IconInActive = "\uf863";
            //                    device.StateActive = "ON";
            //                    device.StateInActive = "OFF";
            //                    break;

            //                case "light":
            //                    device.IconActive = "\uf672";
            //                    device.IconInActive = "\uf0eb";
            //                    device.StateActive = "ON";
            //                    device.StateInActive = "OFF";
            //                    break;

            //                default:
            //                    device.IconActive = "\uf2db";
            //                    device.IconInActive = "\uf2db";
            //                    device.StateActive = "ENABLE";
            //                    device.StateInActive = "DISABLE";
            //                    break;
            //            }

            //            _deviceItems.Add(device);
            //        }
            //        else { }
            //    }
            //}
            //else
            //{
            //    _deviceItems.Clear();
            //}
        }
    }
}
