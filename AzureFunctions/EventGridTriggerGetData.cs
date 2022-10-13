// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AzureFunctions
{
    public static class EventGridTriggerGetData
    {
        static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubEndpoint"));
        [FunctionName("EventGridTriggerGetData")]
        public static async Task Run(EventData message, ILogger log)
        {
            //log.LogInformation($"\nSystemProperties:\n\t{string.Join(" | ", message.SystemProperties.Select(i => $"{i.Key}={i.Value}"))}");

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
