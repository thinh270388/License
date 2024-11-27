using License.Desktop.Helpers;
using License.Desktop.Services.Constracts;
using License.Desktop.Views;
using License.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace License.Desktop.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        private string imgPath = "/License.Desktop;component/Resources/Images/";
        private string logo = "/License.Desktop;component/Resources/Images/logo.png";

        private readonly INavigationService _navigationService;
        public INavigationService NavigationService => _navigationService;  // Binding to ContentControl
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<MenuControl> menuControls = new();
        private bool isMenuExpanded = true;
        private int menuWidth = 200;
        private string currentTime = string.Empty;
        private string version = string.Empty;
        private ApplicationUser user = null!;

        public RelayCommand ToggleMenuCommand => new RelayCommand(cmd => ToggleMenu(), canExecute => true);
        public RelayCommand HomeCommand => new RelayCommand(cmd => NavigationService.NavigateTo<HomeViewModel>(), canExecute => true);
        public RelayCommand UserCommand => new RelayCommand(cmd => NavigationService.NavigateTo<UserViewModel>(), canExecute => true);
        public RelayCommand CertificateCommand => new RelayCommand(cmd => NavigationService.NavigateTo<CertificateViewModel>(), canExecute => true);
        public RelayCommand LogoutCommand => new RelayCommand(cmd => LogoutClick(), canExecute => true);

        public MainViewModel(INavigationService navigationService, IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            MenuControls = new()
            {                
                new MenuControl() { ItemIcon = imgPath + "homes.png", ItemText = "Trang chủ", IconText = "&#xE80F;", OnClicked = HomeCommand },
                new MenuControl() { ItemIcon = imgPath + "users.png", ItemText = "Người dùng", IconText = "&#xEA8C;", OnClicked = UserCommand },
                new MenuControl() { ItemIcon = imgPath + "certificate.png", ItemText = "Giấy phép", IconText = "&#xE83D;", OnClicked = CertificateCommand },
            };

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) => CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            timer.Start();

            NavigationService.NavigateTo<UserViewModel>();

            Version = $"v{Assembly.GetExecutingAssembly()!.GetName()!.Version}";
        }
        private void ToggleMenu()
        {
            IsMenuExpanded = !IsMenuExpanded;
            MenuWidth = IsMenuExpanded ? 200 : 60;
        }
        private void LogoutClick()
        {
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            var viewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginWindow.DataContext = viewModel;
            loginWindow.Show();

            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()!.Close();
        }
        public ObservableCollection<MenuControl> MenuControls { get => menuControls; set => SetProperty(ref menuControls, value); }
        public string Logo { get => logo; set => SetProperty(ref logo, value); }
        public bool IsMenuExpanded { get => isMenuExpanded; set => SetProperty(ref isMenuExpanded, value); }
        public string CurrentTime { get => currentTime; set => SetProperty(ref currentTime, value); }
        public int MenuWidth { get => menuWidth; set => SetProperty(ref menuWidth, value); }
        public string Version { get => version; set => version = value; }
        public ApplicationUser User { get => user; set => SetProperty(ref user, value); }
    }
}
