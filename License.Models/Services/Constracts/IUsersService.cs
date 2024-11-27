using License.Models.DTOs;
using License.Models.Entities;
using License.Models.Responses;

namespace License.Models.Services.Constracts
{
    public interface IUsersService
    {
        Task<DtoResult<ApplicationUser>> GetAllAsync();
        Task<DtoResult<ApplicationUser>> UpdateAsync(ApplicationUser model);
        Task<DtoResult<ApplicationUser>> DeleteAsync(Guid id);
        Task<DtoResult<ApplicationUser>> RegisterAsync(RegisterModel model);
        Task<DtoResult<LoginResponse>> LoginAsync(LoginModel model);
        Task<DtoResult<LoginResponse>> RefreshTokenAsync(RefreshTokenModel model);
    }
}
