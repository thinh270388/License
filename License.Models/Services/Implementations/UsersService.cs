using License.Models.DTOs;
using License.Models.Entities;
using License.Models.Helpers;
using License.Models.Responses;
using License.Models.Services.Constracts;
using System.Net.Http.Json;

namespace License.Models.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrlHelper.URL_API);
        }
        public async Task<DtoResult<ApplicationUser>> RegisterAsync(RegisterModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Users/Register", model);
            if (!result.IsSuccessStatusCode) 
                return new DtoResult<ApplicationUser>() { Success = false, Message = "Error occured" };

            return (await result.Content.ReadFromJsonAsync<DtoResult<ApplicationUser>>())!;
        }
        public async Task<DtoResult<LoginResponse>> LoginAsync(LoginModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Users/Login", model);
            if (!result.IsSuccessStatusCode)
                return new DtoResult<LoginResponse> { Success = false, Message = "Error occured" };

            return (await result.Content.ReadFromJsonAsync<DtoResult<LoginResponse>>())!;
        }
        public async Task<DtoResult<LoginResponse>> RefreshTokenAsync(RefreshTokenModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Users/RefreshToken", model);
            if (!result.IsSuccessStatusCode)
                return new DtoResult<LoginResponse>() { Success = false, Message = "Error occured" };

            return (await result.Content.ReadFromJsonAsync<DtoResult<LoginResponse>>())!;
        }
        public async Task<DtoResult<ApplicationUser>> GetAllAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<DtoResult<ApplicationUser>>($"Users/GetAll");

            return result!;
        }
        public async Task<DtoResult<ApplicationUser>> GetRolesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<DtoResult<ApplicationUser>>($"Users/Roles");

            return result!;
        }
        public async Task<DtoResult<ApplicationUser>> UpdateAsync(ApplicationUser model)
        {
            var result = await _httpClient.PutAsJsonAsync($"Users/Update", model);
            if (!result.IsSuccessStatusCode)
                return new DtoResult<ApplicationUser>() { Success = false, Message = "Error occured" };

            return (await result.Content.ReadFromJsonAsync<DtoResult<ApplicationUser>>())!;
        }
        public async Task<DtoResult<ApplicationUser>> DeleteAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"Users/Delete/{id})");
            if (!result.IsSuccessStatusCode)
                return new DtoResult<ApplicationUser>() { Success = false, Message = "Error occured" };

            return (await result.Content.ReadFromJsonAsync<DtoResult<ApplicationUser>>())!;
        }
    }
}
