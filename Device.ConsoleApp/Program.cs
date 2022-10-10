using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Shared;
using static Dapper.SqlMapper;
using Device.ConsoleApp.Models;
using System.Net.Http.Json;
using Azure.Messaging.EventHubs.Consumer;

namespace Device.ConsoleApp
{
    class Program
    {
        
        private static DeviceClient deviceClient;
        private static Twin twin;
        private static DeviceItem deviceItem = new DeviceItem();
        private static string apiUri = "http://localhost:7289/api/devices/connect";
        public static async Task Main()
        {
            await GetConfigurationAsync();
            //if (string.IsNullOrEmpty(deviceItem.DeviceId))
            //    SetSettings();
            //await SetIntervalAsync();
            //await SetDeviceTwinAsync();
            SendDeviceToCloudMessagesAsync(deviceClient);
            
            Console.ReadLine();

        }
        private static async Task GetConfigurationAsync()
        {
            Console.Clear();
            Console.WriteLine("Enter your Device's Id: ");

            deviceItem.DeviceId = Console.ReadLine();

            try
            {
                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync(apiUri, new { deviceId = deviceItem.DeviceId });
                if (response.IsSuccessStatusCode)
                {
                    if (response != null)
                    {
                        deviceItem.ConnectionString = await response.Content.ReadAsStringAsync();
                        await InitializeDeviceAsync();
                        Console.WriteLine("Do you want to update properties? y/n");
                        var ans = Console.ReadLine();
                        switch (ans)
                        {
                            case "y":
                                SetSettings();
                                await SetDeviceTwinAsync();
                                break;
                            default:
                                break;
                        }
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Device doesn't exist, do you want to create a new device?y/n");
                    var answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "y":
                            SetSettings();
                            var result = await client.PostAsJsonAsync(apiUri, new { deviceId = deviceItem.DeviceId });
                            deviceItem.ConnectionString = await response.Content.ReadAsStringAsync();
                            await InitializeDeviceAsync();
                            await SetDeviceTwinAsync();
                            break;
                        default:
                            break;
                    }
                }

            }
            catch { }


        }
        private static void SetSettings()
        {
            Console.Clear();
            Console.WriteLine("##### Device Settings Configuration #####\n");

            deviceItem.DeviceId = Guid.NewGuid().ToString();
            Console.Write($"DeviceId: {deviceItem.DeviceId}\n");

            Console.Write("Enter Device Name: ");
            deviceItem.DeviceName = Console.ReadLine() ?? "";

            Console.Write("Enter Device Type: ");
            deviceItem.DeviceType = Console.ReadLine() ?? "";

            Console.Write("Enter Location: ");
            deviceItem.Location = Console.ReadLine() ?? "";

            Console.Write("Enter Owner: ");
            deviceItem.Owner = Console.ReadLine() ?? "";
            Console.WriteLine("\n");
        }
        private static async Task InitializeDeviceAsync()
        {
            Console.Write($"\nInitializing device {deviceItem.DeviceId}. Please wait...");

            bool isConfigured = false;

            while (!isConfigured)
            {
                Console.Write(".");

                try
                {
                    deviceClient = DeviceClient.CreateFromConnectionString(deviceItem.ConnectionString, TransportType.Mqtt);
                    twin = await deviceClient.GetTwinAsync();
                }
                catch { }

                if (deviceClient != null && twin != null)
                    isConfigured = true;

                await Task.Delay(500);
            }
        }
        //private static async Task SetIntervalAsync()
        //{
        //    Console.Write($"\nConfiguring sending interval. Please wait...");

        //    try { deviceItem.Interval = (int)twin.Properties.Desired["interval"]; }
        //    catch
        //    {
        //        Console.Write(" - Failed! No interval property found.");
        //    }
        //    await Task.Delay(500);
        //}
        private static async Task SetDeviceTwinAsync()
        {
            Console.WriteLine("Configuring DeviceTwin Properties. Please wait...");

            var twinCollection = new TwinCollection();
            twinCollection["deviceName"] = $"{deviceItem.DeviceName}";
            twinCollection["deviceType"] = $"{deviceItem.DeviceType}";
            twinCollection["deviceState"] = $"{deviceItem.DeviceState}";
            twinCollection["location"] = $"{deviceItem.Location}";
            twinCollection["owner"] = $"{deviceItem.Owner}";            
            await deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }
        private static async void SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient)
        {
            try
            {
                int minTemperature = 20;
                int minHumidity = 60;
                Random rand = new Random();

                while (true)
                {
                    int currentTemperature = (int)(minTemperature + rand.NextDouble() * 15);
                    int currentHumidity = (int)(minHumidity + rand.NextDouble() * 20);

                    // Create JSON message  

                    var telemetryDataPoint = new
                    {

                        temperature = currentTemperature,
                        humidity = currentHumidity
                    };

                    string messageString = "";

                    messageString = JsonConvert.SerializeObject(telemetryDataPoint);

                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    // Add a custom application property to the message.  
                    // An IoT hub can filter on these properties without access to the message body.  
                    //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");  

                    // Send the telemetry message  
                    await s_deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                    await Task.Delay(1000 * 10);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
    }
}