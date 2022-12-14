using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class BaseViewModel : Timers, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public virtual void Dispose() { }
    }
}
