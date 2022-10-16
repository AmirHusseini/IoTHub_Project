using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Device.Lamp
{
    class Program
    {

        private static DeviceClient deviceClient;
        private static Twin twin;
        private static DeviceItem deviceItem = new DeviceItem();
        private static string apiUri = "https://{your azurefunction}.azurewebsites.net/api/devices/connect";
        public static async Task Main()
        {
            await GetConfigurationAsync();
            await SetDirectMethodAsync();
            Console.ReadLine();

        }
        private static async Task GetConfigurationAsync()
        {
            Console.Clear();
            Console.WriteLine("Getting Connectionstring for TV ... ");

            deviceItem.DeviceId = "Lamp";


            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(apiUri, new { deviceId = deviceItem.DeviceId });
            if (response.IsSuccessStatusCode)
            {
                deviceItem.ConnectionString = await response.Content.ReadAsStringAsync();

                deviceClient = DeviceClient.CreateFromConnectionString(deviceItem.ConnectionString, TransportType.Mqtt);
                twin = await deviceClient.GetTwinAsync();
                deviceItem.DeviceId = twin.DeviceId;
                deviceItem.DeviceName = twin.Properties.Reported["deviceName"];
                deviceItem.DeviceType = twin.Properties.Reported["deviceType"];
                deviceItem.Location = twin.Properties.Reported["location"];
                deviceItem.DeviceState = twin.Properties.Reported["deviceState"];

                if (string.IsNullOrEmpty(deviceItem.DeviceType) || string.IsNullOrEmpty(deviceItem.Location))
                {
                    SetSettings();
                    await SetDeviceTwinAsync();
                }
            }
        }
        private static void SetSettings()
        {
            Console.Clear();
            Console.WriteLine($"##### Device Settings Configuration for device {deviceItem.DeviceId} #####\n");

            Console.Write("Enter Device Name: ");
            deviceItem.DeviceName = Console.ReadLine() ?? "";

            Console.Write("Enter Device Type: ");
            deviceItem.DeviceType = Console.ReadLine() ?? "";

            Console.Write("Enter Location: ");
            deviceItem.Location = Console.ReadLine() ?? "";

            Console.WriteLine("\n");


        }

        private static async Task SetDeviceTwinAsync()
        {
            Console.WriteLine("Configuring DeviceTwin Properties. Please wait...");

            var twinCollection = new TwinCollection();
            twinCollection["deviceName"] = $"{deviceItem.DeviceName}";
            twinCollection["deviceType"] = $"{deviceItem.DeviceType}";
            twinCollection["deviceState"] = $"{deviceItem.DeviceState}";
            twinCollection["location"] = $"{deviceItem.Location}";
            twinCollection["interval"] = $"{deviceItem.Interval}";

            await deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }

        private static async Task SetDirectMethodAsync()
        {
            Console.WriteLine("Configuring Direct Method (ON/OFF). Please wait...");
            await deviceClient.SetMethodHandlerAsync("OnOff", OnOff, null);
        }

        private static Task<MethodResponse> OnOff(MethodRequest methodRequest, object userContext)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(methodRequest.DataAsJson);

                Console.WriteLine($"Changing DeviceState from {deviceItem.DeviceState} to {data!.deviceState}.");
                deviceItem.DeviceState = data!.deviceState;

                SetDeviceTwinAsync().ConfigureAwait(false);

                Console.WriteLine($"Device {deviceItem.DeviceId} configured and awaiting new commands.");
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceItem)), 200));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ex.Message)), 400));
            }
        }


    }
}