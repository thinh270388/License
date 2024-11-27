using License.Models;

namespace License.APIs.Repositories.Constracts
{
    public interface IGenericRepository<T>
    {
        Task<DtoResult<T>> GetByIdAsync(Guid id);
        Task<DtoResult<T>> GetAllAsync();
        Task<DtoResult<T>> AddAsync(T entity);
        Task<DtoResult<T>> UpdateAsync(T entity);
        Task<DtoResult<T>> DeleteAsync(Guid id);
    }
}
