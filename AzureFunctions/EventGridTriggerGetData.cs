using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public static class EventGridTriggerGetData
    {
        static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubEndpoint"));
        [FunctionName("EventGridTriggerGetData")]
        public static async Task Run(EventData message, ILogger log)
        {            
            if (message.SystemProperties["iothub-message-source"]?.ToString() == "Telemetry")
            {
                var connectionDeviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
                var msg = Encoding.UTF8.GetString(message.Body.Array);

                log.LogInformation($"DeviceId = {connectionDeviceId}, Telemetry = {msg}");

                var twinPatch = JsonConvert.SerializeObject(new { tags = new { telemetry = new { lastUpdated = message.SystemProperties["iothub-enqueuedtime"], data = JObject.Parse(msg) } } });
                await registryManager.ReplaceTwinAsync(connectionDeviceId, twinPatch, "*");
            }
        }
    }
}
