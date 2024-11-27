using License.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models.Services.Constracts
{
    public interface IGenericService<T>
    {
        Task<DtoResult<T>> GetAllAsync(string endpoint);
        Task<DtoResult<T>> GetByIdAsync(string endpoint, Guid id);
        Task<DtoResult<T>> AddAsync(string endpoint, T item);
        Task<DtoResult<T>> UpdateAsync(string endpoint, T item);
        Task<DtoResult<T>> DeleteAsync(string endpoint, Guid id);
    }
}
