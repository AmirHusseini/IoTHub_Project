using Azure.Messaging.EventHubs.Consumer;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using WPFApp.MVVM.Models;

namespace WPFApp.Services
{
    public interface IGetEventData
    {
        public Task<dynamic> Setup();
    }
    internal class GetEventData : IGetEventData
    {
        private const string EventHubsCompatibleEndpoint = "sb://{ednpoint}.servicebus.windows.net/";
        private const string EventHubsCompatiblePath = "{compatiblepath}";
        private const string IotHubSasKey = "{saskey}";
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

                        var s = Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray());
                        var body = JsonConvert.DeserializeObject<WeatherResponse>(s);
                        return body;
                    }
                
            }
            return null;
        }

    }
}
