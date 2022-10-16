using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WPFApp.MVVM.Models;

namespace WPFApp.Components
{
    /// <summary>
    /// Interaction logic for TileComponent.xaml
    /// </summary>
    public partial class TileComponent : UserControl, INotifyPropertyChanged
    {
        public TileComponent()
        {
            InitializeComponent();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(TileComponent));
        public string DeviceName
        {
            get { return (string)GetValue(DeviceNameProperty); }
            set { SetValue(DeviceNameProperty, value); }
        }

        public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(string), typeof(TileComponent));
        public string DeviceType
        {
            get { return (string)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(TileComponent));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }


        public static readonly DependencyProperty IconActiveProperty = DependencyProperty.Register("IconActive", typeof(string), typeof(TileComponent));

        public string IconActive
        {
            get { return (string)GetValue(IconActiveProperty); }
            set { SetValue(IconActiveProperty, value); }
        }

        public static readonly DependencyProperty IconInActiveProperty = DependencyProperty.Register("IconInActive", typeof(string), typeof(TileComponent));

        public string IconInActive
        {
            get { return (string)GetValue(IconInActiveProperty); }
            set { SetValue(IconInActiveProperty, value); }
        }

        private bool _deviceState;

        public bool DeviceState
        {
            get { return _deviceState; }
            set
            {
                _deviceState = value;
                OnPropertyChanged();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;
                deviceItem.DeviceState = IsChecked;
                

                using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("{IoTHub connectionstring}");

                var directMethod = new CloudToDeviceMethod("OnOff");
                directMethod.SetPayloadJson(JsonConvert.SerializeObject(new { deviceState = IsChecked }));
                await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod); 
            }
            catch { }
        }
    }
}
