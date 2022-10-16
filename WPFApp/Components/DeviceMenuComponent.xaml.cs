using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using WPFApp.MVVM.Models;

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

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(baseurl, new { deviceId = deviceId });
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"New Device {deviceId} added!");
                DeviceNameTxt.Text = "";
            }
        }

        private async void GetDevice_Click(object sender, RoutedEventArgs e)
        {
            deviceId = DeviceIdTxt1.Text;
            using var client = new HttpClient();
            var response = await client.GetAsync(baseurl + deviceId);
            
            if (response.IsSuccessStatusCode)
            {
                
                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
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
            var devices = JsonConvert.DeserializeObject<List<Device>>(await response.Content.ReadAsStringAsync());
            if (devices != null)
            {
                foreach (var item in devices)
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
