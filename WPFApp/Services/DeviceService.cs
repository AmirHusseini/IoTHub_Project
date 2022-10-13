using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WPFApp.MVVM.Models;

namespace WPFApp.Services
{
    internal interface IDeviceService
    {
        public Task<List<DeviceItem>> GetDevicesAsync(string query);
        public Task<Device> AddDeviceAsync(DeviceItem deviceItem);
        public Task RemoveDeviceAsync(string id);
        public Task<Device> GetDeviceAsync(string id);
    }

    internal class DeviceService : IDeviceService
    {
        private readonly string connectionString = "HostName=kyh-iothub-2.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=oj0gqypXDJVBFRzURDHu3zM7Xcu0H2AXRwl7o9/JMiw=";
        private readonly string baseurl = "https://iotproject-webapi.azurewebsites.net/api/devices/";

        public Task<Device> AddDeviceAsync(DeviceItem deviceItem)
        {
            throw new NotImplementedException();
        }

        public async Task<Device> GetDeviceAsync(string id)
        {
            using var client = new HttpClient();
            var device = await client.GetFromJsonAsync<Device>(baseurl + id);
            return device;
        }

        public async Task<List<DeviceItem>> GetDevicesAsync(string query)
        {
            var devices = new List<DeviceItem>();

            try
            {
                using var registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                var result = registryManager.CreateQuery(query);

                if (result.HasMoreResults)
                {
                    foreach (var twin in await result.GetNextAsTwinAsync())
                    {
                        var device = new DeviceItem
                        {
                            DeviceId = twin.DeviceId
                        };

                        try { device.DeviceName = twin.Properties.Reported["deviceName"].ToString(); }
                        catch { }
                        try { device.DeviceType = twin.Properties.Reported["deviceType"].ToString(); }
                        catch { }
                        try { device.Location = twin.Properties.Reported["location"].ToString(); }
                        catch { }
                        try { device.Owner = twin.Properties.Reported["owner"].ToString(); }
                        catch { }
                        try { device.DeviceState = twin.Properties.Reported["deviceState"]; }
                        catch { }

                        switch (device.DeviceType.ToLower())
                        {
                            case "fan":
                                device.IconActive = "\uf863";
                                device.IconInActive = "\uf863";
                                break;

                            case "light":
                                device.IconActive = "\uf672";
                                device.IconInActive = "\uf0eb";
                                break;

                            case "tv":
                                device.IconActive = "\uf8e6";
                                device.IconInActive = "\uf26c";
                                break;

                            default:
                                device.IconActive = "\uf2db";
                                device.IconInActive = "\uf2db";
                                break;
                        }

                        devices.Add(device);
                    }
                }
            }
            catch { }

            return devices;
        }

        public async Task RemoveDeviceAsync(string id)
        {
            using var client = new HttpClient();
            var device = await client.DeleteAsync(baseurl + id);
           
        }
    }
}
