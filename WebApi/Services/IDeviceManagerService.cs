using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace WebApi.Services
{
    public interface IDeviceManagerService
    {
        public Task<Device> CreateDeviceAsync(string deviceId);        
        public Task<Device> GetDeviceByIdAsync(string deviceId);
        public Task<Twin> GetDeviceTwinByIdAsync(string deviceId);
        public Task<IEnumerable<Twin>> GetAllDevicesTwinAsync(string query = "SELECT * FROM devices");
        public Task<bool> DeleteDeviceAsync(string deviceId);
        public Task<IEnumerable<Device>> GetAllDevicesAsync();
    }
}
