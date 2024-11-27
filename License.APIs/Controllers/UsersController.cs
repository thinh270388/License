using License.APIs.Repositories.Constracts;
using License.Models;
using License.Models.DTOs;
using License.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace License.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _repository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                return Ok(await _repository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddAsync(RegisterModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _repository.AddAsync(item));
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAdministrator")]
        public async Task<IActionResult> AddAdministratorAsync(RegisterModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _repository.AddAdministratorAsync(item));
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync(ApplicationUser item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _repository.UpdateAsync(item));
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                return Ok(await _repository.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _repository.LoginAsync(item));
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _repository.RefreshTokenAsync(item));
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RevokeUser")]
        [Authorize]
        public async Task<IActionResult> RevokeUserAsync(string email)
        {
            try
            {
                return Ok(await _repository.RevokeUserAsync(email));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RevokeAll")]
        public async Task<IActionResult> RevokeAllAsync()
        {
            try
            {
                return Ok(await _repository.RevokeAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 

}
