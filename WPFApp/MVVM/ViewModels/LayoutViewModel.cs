using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.MVVM.ViewModels
{
    internal class LayoutViewModel : BaseViewModel
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public BaseViewModel ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, BaseViewModel contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
