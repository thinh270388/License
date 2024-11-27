using License.Models;
using License.Models.DTOs;
using License.Models.Entities;
using License.Models.Responses;

namespace License.APIs.Repositories.Constracts
{
    public interface IUserRepository
    {
        Task<DtoResult<ApplicationUser>> GetAllAsync();
        Task<DtoResult<ApplicationUser>> GetByIdAsync(Guid id);
        Task<DtoResult<ApplicationUser>> AddAsync(RegisterModel model);
        Task<DtoResult<ApplicationUser>> UpdateAsync(ApplicationUser model);
        Task<DtoResult<ApplicationUser>> DeleteAsync(Guid id);
        Task<DtoResult<ApplicationUser>> AddAdministratorAsync(RegisterModel model);
        Task<DtoResult<LoginResponse>> LoginAsync(LoginModel model);
        Task<DtoResult<LoginResponse>> RefreshTokenAsync(RefreshTokenModel model);
        Task<DtoResult<GeneralResponse>> RevokeUserAsync(string email);
        Task<DtoResult<GeneralResponse>> RevokeAllAsync();
    }
}
