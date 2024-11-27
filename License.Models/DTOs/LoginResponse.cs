using License.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models.DTOs
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public ApplicationUser? UserLogin { get; set; }
        public string? UserRole { get; set; }
    }
}
