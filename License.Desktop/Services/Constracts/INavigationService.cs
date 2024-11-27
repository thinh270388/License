using License.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Desktop.Services.Constracts
{
    public interface INavigationService
    {
        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
        public BaseViewModel CurrentViewModel { get; set; }
    }
}
