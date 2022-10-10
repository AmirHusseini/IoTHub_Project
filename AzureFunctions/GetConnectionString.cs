using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using Microsoft.AspNetCore.Hosting;

namespace AzureFunctions
{
    public static class GetConnectionString
    {
        [FunctionName("ConnectDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "devices/connect")] HttpRequest req,
            ILogger log)
        {
            try
            {                
                string deviceId = req.Query["deviceId"];

                dynamic data = JsonConvert.DeserializeObject(await new StreamReader(req.Body).ReadToEndAsync());
                deviceId ??= data?.deviceId;

                if (!string.IsNullOrEmpty(deviceId))
                {
                    using var registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
                    var device = await registryManager.GetDeviceAsync(deviceId);
                    device ??= await registryManager.AddDeviceAsync(new Device(deviceId));

                    return new OkObjectResult($"{Environment.GetEnvironmentVariable("IotHub").Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}");
                }

                return new BadRequestObjectResult("deviceId is required");
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }

        }
    }
}
