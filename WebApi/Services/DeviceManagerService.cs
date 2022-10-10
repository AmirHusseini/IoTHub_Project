using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Rest;

namespace WebApi.Services
{
    public class DeviceManagerService : IDeviceManagerService
    {
        private readonly IConfiguration _config;
        private readonly RegistryManager _registryManager;
        
        public DeviceManagerService(IConfiguration config)
        {
            _config = config;
            _registryManager = RegistryManager.CreateFromConnectionString(_config.GetConnectionString("IoTHub"));
        }

        public async Task<Device> CreateDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager.AddDeviceAsync(new Device(deviceId));
                
                if (device != null)
                {
                    return device;
                }
            }

            catch (Exception)
            {

                throw;
            }            
            
            return null!;
        }

        public async Task<Device> GetDeviceByIdAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager.GetDeviceAsync(deviceId);

                if (device != null)
                {
                    return device;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null!;
        }

        public async Task<Twin> GetDeviceTwinByIdAsync(string deviceId)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync((await _registryManager.GetDeviceAsync(deviceId)).Id);

                if (twin != null)
                {
                    return twin;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null!;
        }
        public async Task<IEnumerable<Twin>> GetAllDevicesTwinAsync(string query = "SELECT * FROM devices")
        {
            var devices = new List<Twin>();

            try
            {
                var result = _registryManager.CreateQuery(query);
                
                if (result.HasMoreResults)
                {
                    foreach (var twin in await result.GetNextAsTwinAsync())
                    {
                        devices.Add(twin);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return devices;
        }

        [Obsolete]
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var devices = new List<Device>();

            try
            {
                var result = await _registryManager.GetDevicesAsync(1000);

                if (result != null)
                {
                    foreach (var device in result)
                    {
                        devices.Add(device);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return devices;
        }
        public async Task<bool> DeleteDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager.GetDeviceAsync(deviceId);
                await _registryManager.RemoveDeviceAsync(device);
                return true;
            }
            catch (Exception)
            { }
            return false;
        }
    }
}

