using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.MVVM.Models;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class LivingroomViewModel : BaseViewModel
    {       
        private readonly INavigationService _navigationStore;
        private readonly IDeviceService _deviceService;
        private readonly IGetCurrentWeatherOutside _weatherOutside;

        public LivingroomViewModel(INavigationService navigationStore, IDeviceService deviceService, IGetCurrentWeatherOutside weatherOutside)
        {            
            _navigationStore = navigationStore;
            _deviceService = deviceService;
            _weatherOutside = weatherOutside;

            DeviceItems = new ObservableCollection<DeviceItem>();

            SetClock();
            SetWeatherAsync().ConfigureAwait(false);
            PopulateDeviceItemsAsync().ConfigureAwait(false);
            
        }


        public string Title => "Living Room";
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
        private string? _icon;
        public string Icon
        {
            get => _icon!;
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }
        protected override async void second_timer_tick(object? sender, EventArgs e)
        {
            SetClock();
            await PopulateDeviceItemsAsync();            
            base.second_timer_tick(sender, e);
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
        private async Task SetWeatherAsync()
        {            
            var weather = await _weatherOutside.GetWeatherDataAsync();
            CurrentTemperature = ((int)weather.Temperature - 273).ToString();
            CurrentHumidity = weather.Humidity.ToString();
            Icon = $"http://openweathermap.org/img/wn/{weather.Icon}@2x.png";
        }
        private async Task PopulateDeviceItemsAsync()
        {
            var result = await _deviceService.GetDevicesAsync("SELECT * FROM devices where properties.reported.location = 'livingroom'");

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
