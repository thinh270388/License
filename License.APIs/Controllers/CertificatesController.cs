using License.APIs.Repositories.Constracts;
using License.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace License.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(IGenericRepository<Certificate> repository) : GenericController<Certificate>(repository)
    {
    }
}
