using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFApp.Helpers;
using WPFApp.MVVM.ViewModels;
using WPFApp.Services;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? app { get; private set; }

        public App()
        {
            app = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {

                services.AddSingleton<MainWindow>();
                services.AddSingleton<NavigationStore>();
                services.AddScoped<IDeviceService, DeviceService>();
                services.AddScoped<IGetEventData, GetEventData>();

            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var navigationStore = app!.Services.GetRequiredService<NavigationStore>();
            var deviceService = app!.Services.GetRequiredService<IDeviceService>();
            var getEventData = app!.Services.GetRequiredService<IGetEventData>();
            navigationStore.CurrentViewModel = new KitchenViewModel(navigationStore, deviceService, getEventData);

            await app!.StartAsync();
            var MainWindow = app.Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = new MainViewModel(navigationStore);
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await app!.StopAsync();
            base.OnExit(e);
        }
    }
}
