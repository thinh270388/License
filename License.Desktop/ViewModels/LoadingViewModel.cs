using License.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Desktop.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public void ShowLoading()
        {
            IsLoading = true;
        }

        public void HideLoading()
        {
            IsLoading = false;
        }
    }
}
