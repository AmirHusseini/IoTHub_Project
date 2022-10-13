using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WebApi.Services;
using WPFApp.MVVM.Models;
using WPFApp.MVVM.ViewModels;

namespace WPFApp.Components
{
    /// <summary>
    /// Interaction logic for DeviceMenuComponent.xaml
    /// </summary>
    public partial class DeviceMenuComponent : UserControl
    {
        private string deviceId = "";
        private readonly string baseurl = "https://iotproject-webapi.azurewebsites.net/api/devices/";
        private List<DeviceItem> Devices = new List<DeviceItem>();
        public DeviceMenuComponent()
        {
            InitializeComponent();            
        }

        

        private async void AddaNewDevice_Click(object sender, RoutedEventArgs e)
        {
            var deviceId = Guid.NewGuid();
            //string deviceName = DeviceNameTxt.Text;
            //var deviceState = false;
            //string deviceType = DeviceTypetxt.Text;
            //string location = Locationtxt.Text;
            //int interval = 
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(baseurl, new { deviceId = deviceId });
            if (response.IsSuccessStatusCode)
            {
                //var result = await response.Content.ReadFromJsonAsync<Device>();
                //DeviceClient deviceClient = DeviceClient.CreateFromConnectionString($"{result.Authentication.SymmetricKey.PrimaryKey}");
                //deviceClient.GetTwinAsync().Wait();
                //TwinCollection twinCollection = new TwinCollection();
                //twinCollection["deviceName"] = $"{deviceName}";
                //twinCollection["deviceType"] = $"{deviceType}";
                //twinCollection["deviceState"] = $"{deviceState}";
                //twinCollection["location"] = $"{location}";
                //twinCollection["interval"] = $"{deviceItem.Interval}";
                MessageBox.Show($"New Device {deviceId} added!");
                DeviceNameTxt.Text = "";
            }
        }

        private async void GetDevice_Click(object sender, RoutedEventArgs e)
        {
            deviceId = DeviceIdTxt1.Text;
            using var client = new HttpClient();
            var response = await client.GetAsync(baseurl + deviceId);
            var response2 = await client.GetAsync(baseurl +"twin/" + deviceId);
            
            if (response.IsSuccessStatusCode)
            {
                
                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                var data2 = JsonConvert.DeserializeObject<dynamic>(await response2.Content.ReadAsStringAsync());
                if (data != null)
                {

                    deviceId = data.id;
                    lbldeviceID.Content = deviceId;
                    DeviceIdTxt1.Text = "";
                }
                
            }
            
        }

        private async void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Are you sure you want to delete this device?";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;
            deviceId = DeviceIdTxt2.Text;
            using var client = new HttpClient();
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:                   
                    var response = await client.DeleteAsync(baseurl + deviceId);
                    if (response.IsSuccessStatusCode)
                    {
                        DeviceIdTxt2.Text = "";
                    }   
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private async void GetAllDevices_Click(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(baseurl + "twins");
            var response2 = await client.GetAsync(baseurl);
            var devices = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(await response.Content.ReadAsStringAsync());
            var devices2 = JsonConvert.DeserializeObject<List<Device>>(await response.Content.ReadAsStringAsync());
            if (devices2 != null)
            {
                foreach (var item in devices2)
                {
                    var device = new DeviceItem();
                    device.DeviceId = item.Id;
                    Devices.Add(device);
                }
                
            }
            lbTodoList.ItemsSource = Devices;
        }           
    }
}
