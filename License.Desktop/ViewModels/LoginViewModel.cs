using License.Desktop.Helpers;
using License.Desktop.Views;
using License.Models.DTOs;
using License.Models.Services.Constracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;

namespace License.Desktop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUsersService _usersService;
        private LoginModel loginModel = new();
        private string errorMessage = string.Empty;
        private bool remember = false;
        RegistryHelper registry = new();
        public RelayCommand LoginCommand => new RelayCommand(cmd => Login(), canExecute => !string.IsNullOrEmpty(LoginModel.Email) && !string.IsNullOrEmpty(LoginModel.Password));
        public RelayCommand CancelCommand => new RelayCommand(cmd => Cancel(), canExecute => true);

        public LoginViewModel(IServiceProvider serviceProvider, IUsersService usersService)
        {
            _serviceProvider = serviceProvider;
            _usersService = usersService;

            string s = registry.Read(ConstantHelper.REGISTRY_SUBKEY, ConstantHelper.REGISTRY_LOGIN, false)!;
            if (!string.IsNullOrEmpty(s))
            {
                SaveLoginModel isSave = JsonConvert.DeserializeObject<SaveLoginModel>(s)!;
                LoginModel.Email = isSave.Email;
                LoginModel.Password = isSave.Password;
                Remember = isSave.IsRemember;
            }
        }
        private async void Login()
        {
            var result = await _usersService.LoginAsync(LoginModel);
            if (result.Success)
            {
                if (Remember)
                {
                    registry.Write(ConstantHelper.REGISTRY_SUBKEY, ConstantHelper.REGISTRY_LOGIN, JsonConvert.SerializeObject(new SaveLoginModel() { IsRemember = Remember , Email = LoginModel.Email, Password = LoginModel.Password}), false);
                }
                else
                {
                    registry.Write(ConstantHelper.REGISTRY_SUBKEY, ConstantHelper.REGISTRY_LOGIN, JsonConvert.SerializeObject(new SaveLoginModel()), false);
                }    

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                var viewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                viewModel.User = result.Result!.UserLogin!;
                mainWindow.DataContext = viewModel;
                
                mainWindow.Show();

                Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()!.Close();
            }    
           else
            {
                ErrorMessage = result.Message;
            }    
        }
        private void Cancel()
        {
            Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()!.Close();
        }
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
        public LoginModel LoginModel { get => loginModel; set => SetProperty(ref loginModel, value); }
        public bool Remember { get => remember; set => SetProperty(ref remember, value); }
    }
}
