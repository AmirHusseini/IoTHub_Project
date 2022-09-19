using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult(await _deviceManager.GetAllDevicesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var device = await _deviceManager.GetDeviceByIdAsync(id);
            return device != null ? new OkObjectResult(device) : new NotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(DeviceRequest device)
        {
            var key = await _deviceManager.CreateDeviceAsync(device.DeviceId);
            return key != null ? new OkObjectResult(key) : new BadRequestResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(string id)
        {
            var result = await _deviceManager.DeleteDeviceAsync(id);
            return result ? new OkResult() : new BadRequestResult();
        }
    }
}
