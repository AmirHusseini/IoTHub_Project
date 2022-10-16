using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebApi.Services;
using WPFApp.MVVM.Cores;
using WPFApp.MVVM.Models;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class DeviceManagementViewModel : BaseViewModel
    {        
        private readonly IDeviceService _deviceService;
        private readonly INavigationService _navigationStore;

        private DispatcherTimer timer;
        private Device device;

        public DeviceManagementViewModel(INavigationService navigationStore, IDeviceService deviceService)
        {
            DeviceItems = new ObservableCollection<DeviceItem>();
            _navigationStore = navigationStore;
            _deviceService = deviceService;

            //SetClock();
            //PopulateDeviceItemsAsync().ConfigureAwait(false);
        }


        public string Title => "Device Management";

        private ObservableCollection<DeviceItem>? _deviceItems;
        public ObservableCollection<DeviceItem>? DeviceItems
        {
            get => _deviceItems;
            set
            {
                _deviceItems = value;
                OnPropertyChanged();
            }
        }
        private string? _currentTime;
        public string CurrentTime
        {
            get => _currentTime!;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private string? _currentDate;
        public string CurrentDate
        {
            get => _currentDate!;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }
        private string? _currentTemperature;
        public string CurrentTemperature
        {
            get => _currentTemperature!;
            set
            {
                _currentTemperature = value;
                OnPropertyChanged();
            }
        }
        private string? _currentHumidity;
        public string CurrentHumidity
        {
            get => _currentHumidity!;
            set
            {
                _currentHumidity = value;
                OnPropertyChanged();
            }
        }
        private string? _currentWeatherCondition;
        public string CurrentWeatherCondition
        {
            get => _currentWeatherCondition!;
            set
            {
                _currentWeatherCondition = value;
                OnPropertyChanged();
            }
        }
        protected override async void second_timer_tick(object? sender, EventArgs e)
        {
            SetClock();
            await PopulateDeviceItemsAsync();
            base.second_timer_tick(sender, e);
        }

        private string _deviceId;

        public string deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        private void SetClock()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
        }

        private async Task AddDeviceAsync()
        {

            device = await _deviceService.GetDeviceAsync(deviceId); 
        }

        private async Task RemoveDeviceAsync()
        {

            var s = await _deviceService.GetDeviceAsync(deviceId);
        }

        private async Task PopulateDeviceItemsAsync()
        {
            var result = await _deviceService.GetDevicesAsync("SELECT * FROM devices");

            result.ForEach(device =>
            {
                var item = DeviceItems?.FirstOrDefault(x => x.DeviceId == device.DeviceId);
                if (item == null)
                    DeviceItems?.Add(device);
                else
                {
                    var index = _deviceItems!.IndexOf(item);
                    _deviceItems[index] = device;
                }
            });
        }

    }
}
