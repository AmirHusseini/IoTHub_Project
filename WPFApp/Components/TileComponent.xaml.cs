using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static readonly DependencyProperty StateActiveProperty = DependencyProperty.Register("StateActive", typeof(string), typeof(TileComponent));

        public string StateActive
        {
            get { return (string)GetValue(StateActiveProperty); }
            set { SetValue(StateActiveProperty, value); }
        }

        public static readonly DependencyProperty StateInActiveProperty = DependencyProperty.Register("StateInActive", typeof(string), typeof(TileComponent));

        public string StateInActive
        {
            get { return (string)GetValue(StateInActiveProperty); }
            set { SetValue(StateInActiveProperty, value); }
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
    }
}
