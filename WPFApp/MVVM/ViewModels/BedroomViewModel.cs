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
    internal class BedroomViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationStore;
        private readonly IDeviceService _deviceService;

        public BedroomViewModel(INavigationService navigationStore, IDeviceService deviceService)
        {
            _navigationStore = navigationStore;
            _deviceService = deviceService;
            DeviceItems = new ObservableCollection<DeviceItem>();

            SetClock();
            PopulateDeviceItemsAsync().ConfigureAwait(false);
        }
        

        public string Title => "Bedroom";
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

        protected override async void second_timer_tick(object? sender, EventArgs e)
        {
            SetClock();
            await PopulateDeviceItemsAsync();
            base.second_timer_tick(sender, e);
        }


        private void SetClock()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
        }


        private async Task PopulateDeviceItemsAsync()
        {
            var result = await _deviceService.GetDevicesAsync("SELECT * FROM devices where properties.reported.location = 'bedroom'");

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
