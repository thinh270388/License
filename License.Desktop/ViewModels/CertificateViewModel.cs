using License.Desktop.Helpers;
using License.Desktop.Views;
using License.Models.Entities;
using License.Models.Services.Constracts;
using License.Models.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;

namespace License.Desktop.ViewModels
{
    public class CertificateViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGenericService<Certificate> _service;
        private readonly LoadingViewModel _loadingViewModel;

        private ObservableCollection<Certificate> certificates = new();
        private Certificate selectedCertificate = null!;
        private int totalRecords;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                _loadingViewModel.IsLoading = _isLoading;
            }
        }
        public RelayCommand AddCommand => new RelayCommand(execute => Add(), canExecute => CanAdd());
        public RelayCommand EditCommand => new RelayCommand((object parameter) => Edit(parameter), canExecute => CanEdit());
        public RelayCommand DeleteCommand => new RelayCommand(async (object parameter) => await Delete(parameter), canExecute => CanDelete());
        public CertificateViewModel(IServiceProvider serviceProvider, 
            IGenericService<Certificate> service,
            LoadingViewModel loadingViewModel)
        {
            _serviceProvider = serviceProvider;
            _service = service;
            _loadingViewModel = loadingViewModel;

            Task.Run(async () => await LoadCertificates()).Wait();

        }

        public ObservableCollection<Certificate> Certificates { get => certificates; set => SetProperty(ref certificates, value); }
        public Certificate SelectedCertificate { get => selectedCertificate; set => SetProperty(ref selectedCertificate, value); }
        public int TotalRecords { get => totalRecords; set => SetProperty(ref totalRecords, value); }

        private async Task LoadCertificates()
        {
            IsLoading = true;
            var data = await _service.GetAllAsync("Certificates");
            Certificates = new(data.Results!);
            TotalRecords = Certificates.Count;
            Thread.Sleep(3000);
            IsLoading = false;
        }
        private void Add()
        {
            var newCertificate = new Certificate();

            var dialog = _serviceProvider.GetRequiredService<CertificateDialogWindow>();
            var viewModel = _serviceProvider.GetRequiredService<CertificateDialogViewModel>();
            viewModel.Certificate = newCertificate;
            dialog.DataContext = viewModel;
            if (dialog.ShowDialog() == true)
            {
                Certificates.Add(StorageHelper.CurrentCertificate);
            }
        }
        private bool CanAdd() => true;
        private void Edit(object parameter)
        {
            if (parameter is Certificate certificate)
            {
                SelectedCertificate = certificate;

                var clone = SelectedCertificate.Clone();
                var dialog = _serviceProvider.GetRequiredService<CertificateDialogWindow>();
                var viewModel = _serviceProvider.GetRequiredService<CertificateDialogViewModel>();
                viewModel.Certificate = clone;
                dialog.DataContext = viewModel;
                if (dialog.ShowDialog() == true)
                {
                    var index = Certificates.IndexOf(SelectedCertificate);
                    Certificates[index] = clone;
                    SelectedCertificate = clone;
                }
            }
        }
        private bool CanEdit() => SelectedCertificate != null;
        private async Task Delete(object parameter)
        {
            if (parameter is Certificate certificate)
                SelectedCertificate = certificate;
            if (SelectedCertificate == null) return;

            var dialog = MessageBox.Show($"Bạn có chắc muốn xóa giấy phép ({SelectedCertificate.MachineCode})", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialog == MessageBoxResult.Yes)
            {
                var result = await _service.DeleteAsync("Certificates", SelectedCertificate.Id);
                if ((bool)result.Success!)
                {
                    Certificates.Remove(SelectedCertificate);
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool CanDelete() => SelectedCertificate != null;
    }
}
