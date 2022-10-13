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
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();


            services.AddSingleton<NavigationStore>();

            services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IGetCurrentWeatherOutside, GetCurrentWeatherOutside>();
            services.AddScoped<IGetEventData, GetEventData>();
            services.AddTransient<HomeViewModel>();

            services.AddTransient<KitchenViewModel>(s => new KitchenViewModel(
                CreateKitchenNavigationService(s),
                s.GetRequiredService<IDeviceService>(),
                s.GetRequiredService<IGetEventData>()
                ));

            services.AddTransient<BedroomViewModel>(s => new BedroomViewModel(
                CreateBedroomNavigationService(s),
                s.GetRequiredService<IDeviceService>()
                ));

            services.AddTransient<LivingroomViewModel>(s => new LivingroomViewModel(
                CreateLivingroomNavigationService(s), 
                s.GetRequiredService<IDeviceService>(),
                s.GetRequiredService<IGetCurrentWeatherOutside>()));

            services.AddTransient<DeviceManagementViewModel>(s => new DeviceManagementViewModel(
                CreateDeviceNavigationService(s),
                s.GetRequiredService<IDeviceService>()
                ));

            services.AddTransient<NavigationBarViewModel>(CreateNavigationBarViewModel);

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>(),                
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }


        private INavigationService CreateLivingroomNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<LivingroomViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LivingroomViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateKitchenNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<KitchenViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<KitchenViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateBedroomNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<BedroomViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<BedroomViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }
        private INavigationService CreateDeviceNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<DeviceManagementViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<DeviceManagementViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateHomeNavigationService(serviceProvider),
                CreateKitchenNavigationService(serviceProvider),
                CreateBedroomNavigationService(serviceProvider),
                CreateLivingroomNavigationService(serviceProvider),
                CreateDeviceNavigationService(serviceProvider)
                );
        }

        
    }
}
