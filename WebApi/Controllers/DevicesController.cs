using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceManagerService _deviceManager;

        public DevicesController(IDeviceManagerService deviceManager)
        {
            _deviceManager = deviceManager;
        }

        [HttpGet("twins")]
        public async Task<IActionResult> GetAllTwins()
        {
            var twins = await _deviceManager.GetAllDevicesTwinAsync();
            return twins != null ? new OkObjectResult(twins) : new NotFoundResult();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _deviceManager.GetAllDevicesAsync();
            return devices != null ? new OkObjectResult(devices) : new NotFoundResult();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var device = await _deviceManager.GetDeviceByIdAsync(id);
            return device != null ? new OkObjectResult(device) : new NotFoundResult();
        }

        [HttpGet("twins/{id}")]
        public async Task<IActionResult> GetDeviceTwinById(string id)
        {
            var twin = await _deviceManager.GetDeviceTwinByIdAsync(id);
            return twin != null ? new OkObjectResult(twin) : new NotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(DeviceRequest device)
        {
            var result = await _deviceManager.CreateDeviceAsync(device.DeviceId);
            return result != null ? new OkObjectResult(result) : new BadRequestResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(string id)
        {
            var result = await _deviceManager.DeleteDeviceAsync(id);
            return result ? new OkResult() : new BadRequestResult();
        }
    }
}
