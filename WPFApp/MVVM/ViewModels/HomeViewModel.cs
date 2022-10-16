using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.MVVM.Models;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class HomeViewModel : BaseViewModel
    {
        public string WelcomeMessage => "Home Page";
        private readonly IGetCurrentWeatherOutside _getCurrentWeather;
        private readonly INavigationService _navigationStore;
        public HomeViewModel(INavigationService navigationStore, IGetCurrentWeatherOutside getCurrentWeather)
        {
            _navigationStore = navigationStore;
            _getCurrentWeather = getCurrentWeather;            
            SetClock();
            SetTemperatureAsync().ConfigureAwait(false);
            
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
        private async Task SetTemperatureAsync()
        {
            var weather = await _getCurrentWeather.GetWeatherDataAsync();

            CurrentTemperature = ((int)weather.Temperature - 273).ToString();
            CurrentHumidity = weather.Humidity.ToString();
            Icon = $"http://openweathermap.org/img/wn/{weather.Icon}@2x.png";
        }
        protected override async void second_timer_tick(object? sender, EventArgs e)
        {
            SetClock();            
            base.second_timer_tick(sender, e);
        }
        private void SetClock()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
        }
    }
}
