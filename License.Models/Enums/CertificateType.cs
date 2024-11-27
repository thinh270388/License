using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models.Enums
{
    public enum CertificateType
    {
        [Description("FREE")]
        Free = 0,
        [Description("PRO")]
        Pro = 1,
        [Description("VIP")]
        Vip = 2
    }
}
