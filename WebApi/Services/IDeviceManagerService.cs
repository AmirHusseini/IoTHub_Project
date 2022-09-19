using Microsoft.Azure.Devices.Shared;

namespace WebApi.Services
{
    public interface IDeviceManagerService
    {
        public Task<string> CreateDeviceAsync(string deviceId);
        public Task<Twin> GetDeviceByIdAsync(string deviceId);
        public Task<IEnumerable<Twin>> GetAllDevicesAsync(string query = "SELECT * FROM devices");
        public Task<bool> DeleteDeviceAsync(string deviceId);
    }
}
