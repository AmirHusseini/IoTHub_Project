using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using WPFApp.MVVM.Models;

namespace WPFApp.Services
{
    public interface IGetEventData
    {
        public Task<dynamic> Setup();
    }
    internal class GetEventData : IGetEventData
    {
        private const string EventHubsCompatibleEndpoint = "sb://ihsuprodamres005dednamespace.servicebus.windows.net/";
        private const string EventHubsCompatiblePath = "iothub-ehub-kyh-iothub-21258178-90d1222e5e";
        private const string IotHubSasKey = "oj0gqypXDJVBFRzURDHu3zM7Xcu0H2AXRwl7o9/JMiw=";
        private const string ConsumerGroup = "$Default";
        
        private static EventHubConsumerClient eventHubConsumerClient = null;

        public async Task<dynamic> Setup()
        {
            
            
            string eventHubConnectionString = $"Endpoint={EventHubsCompatibleEndpoint.Replace("sb://", "amqps://")};EntityPath={EventHubsCompatiblePath};SharedAccessKeyName=iothubowner;SharedAccessKey={IotHubSasKey};";
            eventHubConsumerClient = new EventHubConsumerClient(ConsumerGroup, eventHubConnectionString);
            
            var partitions = await eventHubConsumerClient.GetPartitionIdsAsync();
            foreach (string partition in partitions)
            {
                    await foreach (PartitionEvent receivedEvent in eventHubConsumerClient.ReadEventsFromPartitionAsync(partition, EventPosition.Latest))
                    {
                        //WeatherResponse asd = new WeatherResponse();
                        var s = Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray());
                        var body = JsonConvert.DeserializeObject<WeatherResponse>(s);
                        //var result = JsonConvert.DeserializeObject<dynamic>(body);
                        //body = Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray());
                        return body;
                    }
                
            }
            return null;
        }

    }
}
