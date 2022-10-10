using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class GetData
    {
        [FunctionName("GetData")]
        public static async Task Run([EventHubTrigger("GetData", Connection = "IotHubEndpoint")] EventData events,
            [EventHub("dest", Connection = "IotHubEndpoint")] IAsyncCollector<string> outputEvents,
            ILogger log)
        {
            
            log.LogInformation($"Get Data Event Hub trigger function processed a message: {Encoding.UTF8.GetString(events.Body.Array)}");
            await outputEvents.AddAsync(JsonConvert.SerializeObject(events));            
        }
    }
}
