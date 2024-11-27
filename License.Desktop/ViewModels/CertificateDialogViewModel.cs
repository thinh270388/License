using License.Desktop.Helpers;
using License.Desktop.Views;
using License.Models;
using License.Models.Entities;
using License.Models.Enums;
using License.Models.Services.Constracts;
using License.Models.Services.Implementations;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;

namespace License.Desktop.ViewModels
{
    public class CertificateDialogViewModel : BaseViewModel
    {
        private readonly IGenericService<Certificate> _service;
        private Certificate certificate = new();
        private ObservableCollection<ProductType> productTypes = new();
        private ObservableCollection<CertificateType> certificateTypes = new();
        public RelayCommand SaveCommand => new RelayCommand(cmd => Save(), canExecute => true);
        public RelayCommand CancelCommand => new RelayCommand(cmd => Cancel(), canExecute => true);
        public CertificateDialogViewModel(IGenericService<Certificate> service)
        {
            _service = service;

            LoadProductTypes();
            LoadCertificateTypes();
        }

        private async void Save()
        {
            DtoResult<Certificate> result = new();
            if (Certificate.Id == Guid.Empty)
            {
                result = await _service.AddAsync("Certificates", Certificate);
            }
            else
            {
                result = await _service.UpdateAsync("Certificates", Certificate);
            }
            StorageHelper.CurrentCertificate = result.Success ? result.Result! : null!;

            var window = Application.Current.Windows.OfType<CertificateDialogWindow>().FirstOrDefault();
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
        private void Cancel()
        {
            var window = Application.Current.Windows.OfType<CertificateDialogWindow>().FirstOrDefault();
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
        private void LoadProductTypes()
        {
            ProductTypes = new ObservableCollection<ProductType>();
            foreach (ProductType item in Enum.GetValues(typeof(ProductType)))
            {
                ProductTypes.Add(item);
            }
        }
        private void LoadCertificateTypes()
        {
            CertificateTypes = new ObservableCollection<CertificateType>();
            foreach (CertificateType item in Enum.GetValues(typeof(CertificateType)))
            {
                CertificateTypes.Add(item);
            }
        }
        public Certificate Certificate { get => certificate; set => SetProperty(ref certificate, value); }
        public ObservableCollection<ProductType> ProductTypes { get => productTypes; set => SetProperty(ref productTypes, value); }
        public ObservableCollection<CertificateType> CertificateTypes { get => certificateTypes; set => SetProperty(ref certificateTypes, value); }
    }
}
