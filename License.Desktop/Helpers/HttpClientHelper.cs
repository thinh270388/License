using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace License.Desktop.Helpers
{
    public static class HttpClientHelper
    {
        public static readonly HttpClient HttpClient = new HttpClient();
    }
}
