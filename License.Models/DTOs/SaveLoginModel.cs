using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models.DTOs
{
    public class SaveLoginModel : LoginModel
    {
        public bool IsRemember { get; set; } = false;
    }
}
