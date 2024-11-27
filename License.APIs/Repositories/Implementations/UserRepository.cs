using License.APIs.Datas;
using License.APIs.Helpers;
using License.APIs.Repositories.Constracts;
using License.Models;
using License.Models.DTOs;
using License.Models.Entities;
using License.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace License.APIs.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<DtoResult<ApplicationUser>> GetAllAsync()
        {
            var results =  await _context.Users.ToListAsync();
            return new DtoResult<ApplicationUser>() { Success = true, Message = $"GetAll Users {ConstantHelper.SUCCESS}", Results = results };
        }
        public async Task<DtoResult<ApplicationUser>> GetByIdAsync(Guid id)
        {
            var result = await _context.Users.FindAsync(id);
            return new DtoResult<ApplicationUser>() { Success = true, Message = $"GetByID {ConstantHelper.SUCCESS}", Result = result };
        }
        public async Task<DtoResult<ApplicationUser>> AddAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email!);
            if (userExists != null) return new DtoResult<ApplicationUser>() { Success = false, Message = "User registered already" };

            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new DtoResult<ApplicationUser>() { Success = false, Message = "Account creation failed" };

            if (!await _roleManager.RoleExistsAsync(UserRoles.USER))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserRoles.USER });
            await _userManager.AddToRoleAsync(user, UserRoles.USER);

            return new DtoResult<ApplicationUser>() { Success = true, Message = "Account created" };
        }
        public async Task<DtoResult<ApplicationUser>> UpdateAsync(ApplicationUser model)
        {
            var item = await _context.Users.FindAsync(model.Id);
            if (item == null) return new DtoResult<ApplicationUser>() { Success = false, Message = ConstantHelper.NOT_FOUND };

            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.PhoneNumber = model.PhoneNumber;
            item.UserName = model.Email;

            await _context.SaveChangesAsync();
            return new DtoResult<ApplicationUser>() { Success = true, Message = $"Update {ConstantHelper.SUCCESS}", Result = item };
        }
        public async Task<DtoResult<ApplicationUser>> DeleteAsync(Guid id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null) return new DtoResult<ApplicationUser>() { Success = false, Message = ConstantHelper.NOT_FOUND };

            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
            return new DtoResult<ApplicationUser>() { Success = true, Message = $"Delete {ConstantHelper.SUCCESS}" };
        }
        public async Task<DtoResult<LoginResponse>> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email!);
            if (user == null) return new DtoResult<LoginResponse>() { Success = false, Message = $"Login {ConstantHelper.FAILD}. User {ConstantHelper.NOT_FOUND}" };

            if (!await _userManager.CheckPasswordAsync(user!, model.Password!))
                return new DtoResult<LoginResponse>() { Success = false, Message = $"Login {ConstantHelper.FAILD}. Email/Password not valid" };

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, model.Email!),
                new Claim(ClaimTypes.Email, model.Email!),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item.ToString()));
            }

            user.RefreshToken = BuildRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JWT:RefreshTokenValidityInDays"]));
            await _userManager.UpdateAsync(user);

            return new DtoResult<LoginResponse>()
            {
                Success = true,
                Message = $"Login {ConstantHelper.SUCCESS}",
                Result = new LoginResponse() { AccessToken = BuildToken(authClaims), RefreshToken = user.RefreshToken, UserLogin = user, UserRole = userRoles.FirstOrDefault() }
            };
        }
        public async Task<DtoResult<LoginResponse>> RefreshTokenAsync(RefreshTokenModel model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal?.Identity?.Name == null) 
                return new DtoResult<LoginResponse>() { Success = false, Message = "Refresh token could not be generated because user not found" };

            var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
            if (user == null || user!.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return new DtoResult<LoginResponse>() { Success = false, Message = "Refresh token could not be generated because user has not signed in" };

            user.RefreshToken = BuildRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JWT:RefreshTokenValidityInDays"]));
            await _userManager.UpdateAsync(user);
            return new DtoResult<LoginResponse>()
            {
                Success = true,
                Message = $"Token refreshed successfully",
                Result = new LoginResponse() { AccessToken = BuildToken(principal.Claims.ToList()), RefreshToken = user.RefreshToken }
            };
        }
        public async Task<DtoResult<ApplicationUser>> AddAdministratorAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email!);
            if (userExists != null)
                return new DtoResult<ApplicationUser>() { Success = false, Message = $"Email {model.Email} is exists" };

            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded)
            {
                return new DtoResult<ApplicationUser>() { Success = false, Message = $"Create user '{model.Email}' is failed" };
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserRoles.ADMIN });
            }
            await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);

            return new DtoResult<ApplicationUser>() { Success = true, Message = $"Created user '{model.Email}' with role '{UserRoles.ADMIN}'" };
        }
        public async Task<DtoResult<GeneralResponse>> RevokeAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                await _userManager.UpdateAsync(user);
            }
            return new DtoResult<GeneralResponse>() { Success = true, Message = "Revoke all user successfully" };
        }
        public async Task<DtoResult<GeneralResponse>> RevokeUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                await _userManager.UpdateAsync(user);
                return new DtoResult<GeneralResponse>() { Success = true, Message = "Revoke user successfully" };
            }
            return new DtoResult<GeneralResponse>() { Success = false, Message = "Revoke user failed" };
        }
        private string BuildToken(List<Claim> authClaims)
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:TokenValidityInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)), SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string BuildRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
                ValidateLifetime = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
