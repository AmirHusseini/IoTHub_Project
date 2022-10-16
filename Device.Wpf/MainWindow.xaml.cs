using Device.Wpf.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Device.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string apiUri = "https://iotprojectazurefunctions.azurewebsites.net/api/devices/connect";
        private static DeviceItem deviceItem = new DeviceItem();
        private static bool _isConnected = false;
        private static DeviceClient deviceClient;
        private static Twin twin;
        private static string deviceId = "Lamp";
        private static DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            SetDirectMethodAsync().ConfigureAwait(false);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        private static async Task ConnectDevice()
        {

            try
            {
                using var http = new HttpClient();
                var result = await http.PostAsJsonAsync(apiUri, new { deviceId = deviceId });
                if (result.IsSuccessStatusCode)
                {
                    deviceItem.ConnectionString = await result.Content.ReadAsStringAsync();
                    deviceClient = DeviceClient.CreateFromConnectionString(deviceItem.ConnectionString, TransportType.Mqtt);

                    twin = await deviceClient.GetTwinAsync();

                    if (twin.Properties.Reported.Count >= 3)
                    {
                        deviceItem.DeviceId = twin.DeviceId;
                        deviceItem.DeviceName = twin.Properties.Reported["deviceName"];
                        deviceItem.DeviceType = twin.Properties.Reported["deviceType"];
                        deviceItem.Location = twin.Properties.Reported["location"];
                        deviceItem.DeviceState = twin.Properties.Reported["deviceState"];
                        _isConnected = true;
                    }
                    else
                    {
                        await SetDeviceTwinAsync();

                    }
                }

            }
            catch { }
        }

        private static async Task SetDeviceTwinAsync()
        {
            var twinCollection = new TwinCollection();
            twinCollection["deviceName"] = "Living Room Fan";
            twinCollection["deviceType"] = "fan";
            twinCollection["deviceState"] = $"{deviceItem.DeviceState}";
            twinCollection["location"] = "livingroom";
            twinCollection["interval"] = $"{deviceItem.Interval}";

            await deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }
        private static async Task SetDirectMethodAsync()
        {

            await ConnectDevice();
            await deviceClient.SetMethodHandlerAsync("OnOff", OnOff, null);
        }

        private static Task<MethodResponse> OnOff(MethodRequest methodRequest, object userContext)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(methodRequest.DataAsJson);
                deviceItem.DeviceState = data!.deviceState;
                var twinCollection = new TwinCollection();
                twinCollection["deviceState"] = $"{deviceItem.DeviceState}";
                deviceClient.UpdateReportedPropertiesAsync(twinCollection);

                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceItem)), 200));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ex.Message)), 400));
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var iconRotateBladeStoryBoard = (BeginStoryboard)TryFindResource("iconRotateBladeStoryBoard");

            tblockConnectionState.Text = "Device Connected";
            if (deviceItem.DeviceState == true)
            {
                iconRotateBladeStoryBoard.Storyboard.Begin();
            }
            else
            {
                iconRotateBladeStoryBoard.Storyboard.Stop();
            }
        }

    }
}

