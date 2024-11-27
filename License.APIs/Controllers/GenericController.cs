using License.APIs.Repositories.Constracts;
using License.Models;
using Microsoft.AspNetCore.Mvc;

namespace License.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IGenericRepository<T> _repository;
        public GenericController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<DtoResult<T>>> GetAllAsync()
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

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<DtoResult<T>>> GetByIdAsync(Guid id)
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

        [HttpPost("Add")]
        public async Task<ActionResult<DtoResult<T>>> AddAsync(T item)
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

        [HttpPut("Update")]
        public async Task<ActionResult<DtoResult<T>>> UpdateAsync(T item)
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

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<DtoResult<T>>> DeleteAsync(Guid id)
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
    }
}
