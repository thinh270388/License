using Microsoft.AspNetCore.Identity;

namespace License.Models.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
