using eCommerceApp.Services;
using eCommerceApp.Services.Interfaces;
using eCommerceApp.View;
using eCommerceApp.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace eCommerceApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container? Container { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            Register();

            MainStartup();

            base.OnStartup(e);
        }
        private void Register()
        {
            Container = new();

            Container.RegisterSingleton<IMessenger, Messenger>();
            Container.RegisterSingleton<INavigationService, NavigationService>();

            Container.RegisterSingleton<MainViewModel>();
            Container.RegisterSingleton<UserPanelViewModel>();
            Container.RegisterSingleton<ShoppingCartViewModel>();
            Container.RegisterSingleton<RegistrationViewModel>();

            Container.RegisterSingleton<LoginViewModel>();




            Container.Verify();
        }
        private void MainStartup()
        {
            Window mainView = new MainView();

            mainView.DataContext = Container?.GetInstance<MainViewModel>();

            mainView.ShowDialog();
        }
    }
}
