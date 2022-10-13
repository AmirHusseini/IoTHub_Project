using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class NavigationBarViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateKitchenCommand { get; }
        public ICommand NavigateBedroomCommand { get; }
        public ICommand NavigateLivingroomCommand { get; }
        public ICommand NavigateDeviceManagementCommand { get; }

        public NavigationBarViewModel(
            INavigationService homeNavigationService,
            INavigationService kitchenNavigationService,
            INavigationService bedroomNavigationService,
            INavigationService navigateLivingroomCommand,
            INavigationService navigateDeviceManagementCommand)
        {

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateKitchenCommand = new NavigateCommand(kitchenNavigationService);
            NavigateBedroomCommand = new NavigateCommand(bedroomNavigationService);
            NavigateLivingroomCommand = new NavigateCommand(navigateLivingroomCommand);
            NavigateDeviceManagementCommand = new NavigateCommand(navigateDeviceManagementCommand);
        }


        public override void Dispose()
        {

            base.Dispose();
        }
    }
}
