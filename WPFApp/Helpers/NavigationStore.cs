using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.MVVM.ViewModels;

namespace WPFApp.Helpers
{
    internal class NavigationStore
    {
        public event Action? CurrentViewModelChanged;

        private BaseViewModel? _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel!;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
