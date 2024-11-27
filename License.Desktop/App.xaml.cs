using License.Desktop.Services.Constracts;
using License.Desktop.Services.Implementations;
using License.Desktop.ViewModels;
using License.Desktop.Views;
using License.Models.Entities;
using License.Models.Services.Constracts;
using License.Models.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace License.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.DataContext = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginWindow.Show();           
        }
       
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IGenericService<Certificate>, GenericService<Certificate>>();
            services.AddSingleton<IUsersService, UsersService>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<UserViewModel>();
            services.AddTransient<CertificateViewModel>();
            services.AddTransient<CertificateDialogViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<LoadingViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<CertificateDialogWindow>();
        }
    }
}