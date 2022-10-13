using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.MVVM.ViewModels
{
    internal class HomeViewModel : BaseViewModel
    {
        public string WelcomeMessage => "Welcome to my application.";

        //public ICommand NavigateLivingroomCommand { get; }

        public HomeViewModel(/*INavigationService livingroomNavigationService*/)
        {
            //NavigateLivingroomCommand = new NavigateCommand(livingroomNavigationService);

        }

    }
}
