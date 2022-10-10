using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WPFApp.Helpers;
using WPFApp.MVVM.Cores;
using WPFApp.MVVM.Models;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class KitchenViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;               
        private readonly IGetEventData _getEventData;
        private readonly IDeviceService _deviceService;
        public ICommand NavigateToSettings { get; }
        public KitchenViewModel(NavigationStore navigationStore, IDeviceService deviceService, IGetEventData getEventData)
        {
            _navigationStore = navigationStore;
            _getEventData = getEventData;
            _deviceService = deviceService;
            DeviceItems = new ObservableCollection<DeviceItem>();
            NavigateToSettings = new NavigateCommand<KitchenViewModel>(navigationStore, () => new KitchenViewModel(_navigationStore, _deviceService, getEventData));            
            
            SetClock();
            SetWeatherAsync().ConfigureAwait(false);
            PopulateDeviceItemsAsync().ConfigureAwait(false);
        }


        public string Title => "Kitchen";
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
        private async Task SetWeatherAsync()
        {
            WeatherResponse weather = await _getEventData.Setup();
            
            CurrentTemperature = weather.Temperature;
            CurrentHumidity = weather.Humidity;
        }
        protected override async void second_timer_tick(object? sender, EventArgs e)
        {
            SetClock();            
            await PopulateDeviceItemsAsync();
            base.second_timer_tick(sender, e);
        }
        protected override async void minute_timer_tick(object? sender, EventArgs e)
        {
            await SetWeatherAsync();
            base.minute_timer_tick(sender, e);
        }
        protected override async void hour_timer_tick(object? sender, EventArgs e)
        {
            await SetWeatherAsync();
            base.hour_timer_tick(sender, e);
        }


        private void SetClock()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
        }


        private async Task PopulateDeviceItemsAsync()
        {
            var result = await _deviceService.GetDevicesAsync("SELECT * FROM devices where properties.reported.location = 'kitchen'");

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
