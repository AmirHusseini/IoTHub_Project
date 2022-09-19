using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

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

        public async Task<string> CreateDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager.AddDeviceAsync(new Device(deviceId));

                if (device != null)
                {
                    return device.Authentication.SymmetricKey.PrimaryKey;
                }
            }

            catch (Exception)
            {

                throw;
            }            
            
            return null!;
        }

        public async Task<Twin> GetDeviceByIdAsync(string deviceId)
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

        public async Task<IEnumerable<Twin>> GetAllDevicesAsync(string query = "SELECT * FROM devices")
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

